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
        private readonly IHostEnvironment _env;

        public AuthController(IUserService userService, IAdminUserService adminUserService, IHostEnvironment env)
        {
            _userService = userService;
            _adminUserService = adminUserService;
            _env = env;
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
                    SetAuthCookie(result.Token, result.ExpiresAt);
                    result.Token = string.Empty;
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
                SetAuthCookie(result.Token, result.ExpiresAt);
                result.Token = string.Empty;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        // מוחק את הקוקי — הדרך היחידה לנקות HttpOnly cookie היא דרך השרת
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Append("token", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = !_env.IsDevelopment(),
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
            });
            return Ok();
        }

        private void SetAuthCookie(string token, DateTime expiresAt)
        {
            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = !_env.IsDevelopment(), // בפרודקשן HTTPS בלבד
                SameSite = SameSiteMode.Lax,
                Expires = new DateTimeOffset(expiresAt),
            });
        }
    }
}
