using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Exceptions;
using OrlandoServices.Core.Interfaces.Service;

namespace OrlandoServices.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class ServiceController : ControllerBase
    {
        private readonly IServicesService _servicesService;

        public ServiceController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        // לקוח — רשימת שירותים פעילים
        [HttpGet]
        public IActionResult GetAll()
        {
            var services = _servicesService.GetActiveServices();
            return Ok(services);
        }

        // לקוח — שירות ספציפי עם השדות שלו
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var service = _servicesService.GetServiceById(id);
                return Ok(service);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // אדמין — כל השירותים כולל לא פעילים
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/all")]
        public IActionResult GetAllForAdmin()
        {
            var services = _servicesService.GetAllServices();
            return Ok(services);
        }

        // אדמין — שירות ספציפי עם פרטים מלאים
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/{id}")]
        public IActionResult GetByIdForAdmin(int id)
        {
            try
            {
                var service = _servicesService.GetServiceByIdForAdmin(id);
                return Ok(service);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // אדמין — הוספת שירות חדש
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create([FromBody] CreateServiceDto dto)
        {
            try
            {
                _servicesService.CreateService(dto);
                return Ok("Service created successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // אדמין — עדכון שירות קיים
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateServiceDto dto)
        {
            try
            {
                _servicesService.UpdateService(id, dto);
                return Ok("Service updated successfully");
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

        // אדמין — השבתת שירות (לא מחיקה)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _servicesService.DeleteService(id);
                return Ok("Service deactivated successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
