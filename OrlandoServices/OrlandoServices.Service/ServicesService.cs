using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Exceptions;
using OrlandoServices.Core.Interfaces;
using OrlandoServices.Core.Interfaces.Repository;
using OrlandoServices.Core.Interfaces.Service;
using OrlandoServices.Core.Models;
using OrlandoServices.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Service
{
    public class ServicesService : IServicesService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceFieldService _serviceFieldService;
        private readonly IUnitOfWork _unitOfWork;

        public ServicesService(IServiceRepository serviceRepository, IServiceFieldService serviceFieldService,IUnitOfWork unitOfWork)
        {
            _serviceRepository = serviceRepository;
            _serviceFieldService = serviceFieldService;
            _unitOfWork = unitOfWork;
        }
        public void CreateService(CreateServiceDto dto)
        {
            // ולידציה על השירות
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Service name is required");

            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new ArgumentException("Service description is required");

            if (dto.BasePrice < 0)
                throw new ArgumentException("Base price cannot be negative");

            if (dto.Fields == null || dto.Fields.Count == 0)
                throw new ArgumentException("A service must have at least one field");

            // התחלת Transaction
            _unitOfWork.BeginTransaction();

            try
            {
                // יצירת השירות
                var newService = new Services
                {
                    Name = dto.Name.Trim(),
                    Description = dto.Description.Trim(),
                    BasePrice = dto.BasePrice,
                    ImageUrl = string.IsNullOrWhiteSpace(dto.ImageUrl) ? null : dto.ImageUrl.Trim(),
                    Status = ServiceStatus.Active
                };

                _serviceRepository.Add(newService);
                _serviceRepository.SaveChanges(); // ← חשוב! כדי לקבל את ה-Id

                // יצירת השדות — דרך ServiceFieldService שעושה את כל הולידציות
                foreach (var field in dto.Fields)
                {
                    field.ServiceId = newService.Id; // ← מחברים כל שדה לשירות החדש
                    _serviceFieldService.CreateField(field);
                }

                _unitOfWork.Commit(); // הכל הצליח — שומרים
            }
            catch
            {
                _unitOfWork.Rollback(); // משהו נכשל — מבטלים הכל
                throw;
            }
        }
        public void UpdateService(int serviceId, UpdateServiceDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var existingService = _serviceRepository.GetById(serviceId);
            if (existingService == null)
                throw new NotFoundException("Service not found");

            // עדכון חלקי של פרטי השירות
            if (dto.Name != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("Name cannot be empty");
                existingService.Name = dto.Name.Trim();
            }

            if (dto.Description != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Description))
                    throw new ArgumentException("Description cannot be empty");
                existingService.Description = dto.Description.Trim();
            }

            if (dto.BasePrice.HasValue)
            {
                if (dto.BasePrice.Value < 0)
                    throw new ArgumentException("Base price cannot be negative");
                existingService.BasePrice = dto.BasePrice.Value;
            }

            if (dto.ImageUrl != null)
                existingService.ImageUrl = string.IsNullOrWhiteSpace(dto.ImageUrl) ? null : dto.ImageUrl.Trim();

            if (dto.Status.HasValue)
                existingService.Status = dto.Status.Value;

            _unitOfWork.BeginTransaction();
            try
            {
                // הוספת שדות חדשים
                if (dto.NewFields != null && dto.NewFields.Any())
                {
                    foreach (var field in dto.NewFields)
                    {
                        field.ServiceId = existingService.Id;
                        _serviceFieldService.CreateField(field);
                    }
                }

                // עדכון שדות קיימים
                if (dto.UpdatedFields != null && dto.UpdatedFields.Any())
                {
                    foreach (var kvp in dto.UpdatedFields)
                    {
                        int fieldId = kvp.Key;
                        var fieldDto = kvp.Value;
                        _serviceFieldService.UpdateField(fieldId, fieldDto);
                    }
                }

                // מחיקת שדות
                if (dto.FieldIdsToDelete != null && dto.FieldIdsToDelete.Any())
                {
                    foreach (var fieldId in dto.FieldIdsToDelete)
                    {
                        _serviceFieldService.DeleteField(fieldId);
                    }
                }

                // שומרים את השירות
                _serviceRepository.Update(existingService);
                _serviceRepository.SaveChanges();

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
        public void DeleteService(int serviceId)
        {
            var service = _serviceRepository.GetById(serviceId);
            if (service == null)
                throw new NotFoundException("Service not found");

            service.Status = ServiceStatus.Hidden;

            _serviceRepository.Update(service);
            _serviceRepository.SaveChanges();
        }

        public List<ServiceAdminDto> GetAllServices()
        {
            return _serviceRepository.GetAllWithDetails()
                .Select(s => new ServiceAdminDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    BasePrice = s.BasePrice,
                    ImageUrl = s.ImageUrl,
                    Status = s.Status,
                    Fields = s.ServiceFields
                        .OrderBy(f => f.OrderIndex)
                        .Select(f => new ServiceFieldAdminDto
                        {
                            Id = f.Id,
                            ServiceId = f.ServiceId,
                            FieldName = f.FieldName,
                            FieldType = f.FieldType,
                            IsRequired = f.IsRequired,
                            OrderIndex = f.OrderIndex,
                            Options = f.Options,
                            IsActive = f.IsActive,
                            Version = f.Version
                        })
                        .ToList()
                })
                .ToList();
        }
        public List<ServiceClientDto> GetActiveServices()
        {
            return _serviceRepository.GetAllWithDetails()
                .Where(s => s.Status != ServiceStatus.Hidden)
                .Select(s => new ServiceClientDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    BasePrice = s.BasePrice,
                    ImageUrl = s.ImageUrl,
                    Status = s.Status,
                    Fields = s.ServiceFields
                        .Where(f => f.IsActive)
                        .OrderBy(f => f.OrderIndex)
                        .Select(f => new ServiceFieldClientDto
                        {
                            Id = f.Id,
                            FieldName = f.FieldName,
                            FieldType = f.FieldType,
                            IsRequired = f.IsRequired,
                            OrderIndex = f.OrderIndex,
                            Options = f.Options
                        })
                        .ToList()
                })
                .ToList();
        }
        public ServiceClientDto GetServiceById(int id)
        {
            var service = _serviceRepository.GetByIdWithDetails(id);
            if (service == null)
                throw new NotFoundException("Service not found");

            if (service.Status == ServiceStatus.Hidden)
                throw new NotFoundException("Service not found");

            return new ServiceClientDto
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                BasePrice = service.BasePrice,
                ImageUrl = service.ImageUrl,
                Status = service.Status,
                Fields = service.ServiceFields
                    .Where(f => f.IsActive)
                    .OrderBy(f => f.OrderIndex)
                    .Select(f => new ServiceFieldClientDto
                    {
                        Id = f.Id,
                        FieldName = f.FieldName,
                        FieldType = f.FieldType,
                        IsRequired = f.IsRequired,
                        OrderIndex = f.OrderIndex,
                        Options = f.Options
                    })
                    .ToList()
            };
        }
        public ServiceAdminDto GetServiceByIdForAdmin(int id)
        {
            var service = _serviceRepository.GetByIdWithDetails(id);
            if (service == null)
                throw new NotFoundException("Service not found");

            return new ServiceAdminDto
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                BasePrice = service.BasePrice,
                ImageUrl = service.ImageUrl,
                Status = service.Status,
                Fields = service.ServiceFields
                    .OrderBy(f => f.OrderIndex)
                    .Select(f => new ServiceFieldAdminDto
                    {
                        Id = f.Id,
                        ServiceId = f.ServiceId,
                        FieldName = f.FieldName,
                        FieldType = f.FieldType,
                        IsRequired = f.IsRequired,
                        OrderIndex = f.OrderIndex,
                        Options = f.Options,
                        IsActive = f.IsActive,
                        Version = f.Version
                    })
                    .ToList()
            };
        }
    }
}
