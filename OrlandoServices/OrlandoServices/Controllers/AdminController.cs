using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Interfaces.Service;

namespace OrlandoServices.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;
        private readonly IConfiguration _configuration;

        public AdminController(IAdminUserService adminUserService, IConfiguration configuration)
        {
            _adminUserService = adminUserService;
            _configuration = configuration;
        }

        // התחברות אדמין — פתוח לכולם אבל רק אדמין יודע שם משתמש וסיסמה
        [HttpPost("login")]
        public IActionResult Login([FromBody] AdminLoginDto dto)
        {
            try
            {
                var result = _adminUserService.Login(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // יצירת אדמין נוסף — רק אדמין מחובר יכול
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public IActionResult CreateAdmin([FromBody] CreateAdminDto dto)
        {
            try
            {
                _adminUserService.CreateAdmin(dto);
                return Ok("Admin created successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // יצירת האדמין הראשון — מוגן עם מפתח סודי, למחיקה אחרי השימוש
        [HttpPost("setup")]
        public IActionResult Setup([FromBody] CreateAdminDto dto, [FromQuery] string setupKey)
        {
            var expectedKey = _configuration["AdminSetupKey"];
            if (string.IsNullOrEmpty(expectedKey) || setupKey != expectedKey)
                return Unauthorized("Invalid setup key");

            try
            {
                _adminUserService.CreateAdmin(dto);
                return Ok("First admin created successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
