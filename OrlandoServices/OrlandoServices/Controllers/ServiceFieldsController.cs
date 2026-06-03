using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Exceptions;
using OrlandoServices.Core.Interfaces.Service;

namespace OrlandoServices.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/services/{serviceId}/fields")]
    public class ServiceFieldsController : ControllerBase
    {
        private readonly IServiceFieldService _serviceFieldService;

        public ServiceFieldsController(IServiceFieldService serviceFieldService)
        {
            _serviceFieldService = serviceFieldService;
        }

        // אדמין — כל השדות של שירות מסוים
        [HttpGet]
        public IActionResult GetAll(int serviceId)
        {
            var fields = _serviceFieldService.GetAllFieldsByService(serviceId);
            return Ok(fields);
        }

        // אדמין — שדה ספציפי
        [HttpGet("{fieldId}")]
        public IActionResult GetById(int fieldId)
        {
            try
            {
                var field = _serviceFieldService.GetById(fieldId);
                return Ok(field);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // אדמין — הוספת שדה לשירות
        [HttpPost]
        public IActionResult Create(int serviceId, [FromBody] CreateServiceFieldDto dto)
        {
            try
            {
                dto.ServiceId = serviceId;
                _serviceFieldService.CreateField(dto);
                return Ok("Field created successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // אדמין — עדכון שדה קיים
        [HttpPut("{fieldId}")]
        public IActionResult Update(int fieldId, [FromBody] UpdateServiceFieldDto dto)
        {
            try
            {
                _serviceFieldService.UpdateField(fieldId, dto);
                return Ok("Field updated successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // אדמין — מחיקת שדה
        [HttpDelete("{fieldId}")]
        public IActionResult Delete(int fieldId)
        {
            try
            {
                _serviceFieldService.DeleteField(fieldId);
                return Ok("Field deleted successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
