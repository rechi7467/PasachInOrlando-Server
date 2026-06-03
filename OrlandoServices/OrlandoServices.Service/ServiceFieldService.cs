using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Exceptions;
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
    public class ServiceFieldService : IServiceFieldService
    {
        private readonly IServiceFieldRepository _serviceFieldRepository;
        private readonly IServiceRepository _serviceRepository;

        public ServiceFieldService(IServiceFieldRepository serviceFieldRepository, IServiceRepository serviceRepository)
        {
            _serviceFieldRepository = serviceFieldRepository;
            _serviceRepository = serviceRepository;
        }
        public void CreateField(CreateServiceFieldDto field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            if (field.ServiceId <= 0)
                throw new ArgumentException("ServiceId is required");
            var existingService = _serviceRepository.GetById(field.ServiceId);
            if (existingService == null)
                throw new NotFoundException("Service not found");
            if (existingService.Status == ServiceStatus.Hidden)
                throw new InvalidOperationException("Cannot add field to a hidden service");

            if (string.IsNullOrWhiteSpace(field.FieldName))
                throw new ArgumentException("Field Name is required");

            if (!Enum.IsDefined(typeof(FieldType), field.FieldType))
                throw new ArgumentException("Invalid field type");
            if (field.OrderIndex < 0)
                throw new ArgumentException("OrderIndex cannot be negative");
            if ((field.FieldType == FieldType.Select || field.FieldType == FieldType.MultiSelect)
        && string.IsNullOrWhiteSpace(field.Options))
                throw new ArgumentException("Options are required for Select and MultiSelect fields");

            var newField = new ServiceField
            {
                ServiceId = field.ServiceId,
                FieldName = field.FieldName.Trim(),
                FieldType = field.FieldType,
                IsRequired = field.IsRequired,
                OrderIndex = field.OrderIndex,
                Options = field.Options?.Trim(),
                IsActive = true,
                Version = 1
            };

            _serviceFieldRepository.Add(newField);
            _serviceFieldRepository.SaveChanges();
        }
        public void UpdateField(int fieldId, UpdateServiceFieldDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // שליפת השדה הקיים
            var existingField = _serviceFieldRepository.GetById(fieldId);
            if (existingField == null)
                throw new NotFoundException("Field not found");

            // אם השדה לא פעיל — מותר רק להפעיל מחדש
            if (!existingField.IsActive)
            {
                if (dto.IsActive == true)
                {
                    existingField.IsActive = true;
                    _serviceFieldRepository.Update(existingField);
                    _serviceFieldRepository.SaveChanges();
                    return;
                }
                throw new InvalidOperationException("Cannot update an inactive field (except reactivating it)");
            }

            // עדכון חלקי — רק מה שנשלח
            if (dto.FieldName != null)
            {
                if (string.IsNullOrWhiteSpace(dto.FieldName))
                    throw new ArgumentException("FieldName cannot be empty");
                existingField.FieldName = dto.FieldName.Trim();
            }

            if (dto.OrderIndex != null)
            {
                if (dto.OrderIndex.Value < 0)
                    throw new ArgumentException("OrderIndex cannot be negative");
                existingField.OrderIndex = dto.OrderIndex.Value;
            }

            if (dto.IsRequired != null)
                existingField.IsRequired = dto.IsRequired.Value;

            // שינוי סוג שדה — מעלה גרסה כי הזמנות ישנות הסתמכו על הסוג הקודם
            if (dto.FieldType != null && existingField.FieldType != dto.FieldType.Value)
            {
                existingField.FieldType = dto.FieldType.Value;
                existingField.Version++;
            }

            // עדכון Options — רלוונטי רק ל-Select ו-MultiSelect
            if (dto.Options != null)
            {
                var currentType = dto.FieldType.HasValue ? dto.FieldType.Value : existingField.FieldType;
                if (currentType != FieldType.Select && currentType != FieldType.MultiSelect)
                    throw new ArgumentException("Options are only allowed for Select and MultiSelect fields");
                existingField.Options = dto.Options.Trim();
            }

            if (dto.IsActive != null)
                existingField.IsActive = dto.IsActive.Value;

            _serviceFieldRepository.Update(existingField);
            _serviceFieldRepository.SaveChanges();
        }
        public void DeleteField(int fieldId)
        {
            var field = _serviceFieldRepository.GetById(fieldId);
            if (field == null)
                throw new NotFoundException("Field not found");

            if (!field.IsActive)
                throw new InvalidOperationException("Field is already inactive");

            field.IsActive = false;

            _serviceFieldRepository.Update(field);
            _serviceFieldRepository.SaveChanges();
        }
        public List<ServiceFieldClientDto> GetActiveFieldsByService(int serviceId)
        {
            if (serviceId <= 0)
                throw new ArgumentException("Invalid serviceId");

            var existingService = _serviceRepository.GetById(serviceId);
            if (existingService == null)
                throw new NotFoundException("Service not found");
            // אם השירות לא פעיל — מחזירים רשימה ריקה
            if (existingService.Status == ServiceStatus.Hidden)
                throw new InvalidOperationException("Service not found");

            return _serviceFieldRepository.GetByServiceId(serviceId)
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
                .ToList();
        }
        public List<ServiceFieldAdminDto> GetAllFieldsByService(int serviceId)
        {
            if (serviceId <= 0)
                throw new ArgumentException("Invalid serviceId");

            var service = _serviceRepository.GetById(serviceId);
            if (service == null)
                throw new NotFoundException("Service not found");

            return _serviceFieldRepository.GetByServiceId(serviceId)
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
                .ToList();
        }

        public ServiceFieldAdminDto GetById(int fieldId)
        {
            if (fieldId <= 0)
                throw new ArgumentException("Invalid field id");

            var field = _serviceFieldRepository.GetById(fieldId);
            if (field == null)
                throw new NotFoundException("Field not found");

            return new ServiceFieldAdminDto
            {
                Id = field.Id,
                ServiceId = field.ServiceId,
                FieldName = field.FieldName,
                FieldType = field.FieldType,
                IsRequired = field.IsRequired,
                OrderIndex = field.OrderIndex,
                Options = field.Options,
                IsActive = field.IsActive,
                Version = field.Version
            };
        }
    }
}
