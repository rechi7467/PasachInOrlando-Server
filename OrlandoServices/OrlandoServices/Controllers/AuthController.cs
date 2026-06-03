using Microsoft.AspNetCore.Mvc;
using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Interfaces.Service;

namespace OrlandoServices.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAdminUserService _adminUserService;

        public AuthController(IUserService userService, IAdminUserService adminUserService)
        {
            _userService = userService;
            _adminUserService = adminUserService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UnifiedLoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Identifier) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Identifier and password are required.");

            // יש @ — זה אימייל, מנסה כמשתמש רגיל
            if (dto.Identifier.Contains('@'))
            {
                try
                {
                    var result = _userService.Login(new LoginDto
                    {
                        Email = dto.Identifier.Trim(),
                        Password = dto.Password
                    });
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return Unauthorized(ex.Message);
                }
            }

            // אין @ — זה שם משתמש, מנסה כאדמין
            try
            {
                var result = _adminUserService.Login(new AdminLoginDto
                {
                    Username = dto.Identifier.Trim(),
                    Password = dto.Password
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
