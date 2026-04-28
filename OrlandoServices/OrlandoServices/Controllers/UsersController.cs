using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Exceptions;
using OrlandoServices.Core.Interfaces.Service;
using System.Security.Claims;

namespace OrlandoServices.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // התחברות — מחזיר טוקן JWT
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            try
            {
                var result = _userService.Login(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // הגדרת סיסמה — אורח הופך למשתמש רשום
        [HttpPost("set-password")]
        public IActionResult SetPassword([FromBody] SetPasswordDto dto)
        {
            try
            {
                _userService.SetPassword(dto);
                return Ok("Password set successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // פרטי המשתמש המחובר — דורש טוקן תקין
        [Authorize]
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            try
            {
                var user = _userService.GetUserById(int.Parse(userIdClaim));
                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // עדכון פרטי המשתמש המחובר — דורש טוקן תקין
        [Authorize]
        [HttpPut("me")]
        public IActionResult UpdateMe([FromBody] UpdateUserDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            try
            {
                _userService.UpdateUser(int.Parse(userIdClaim), dto);
                return Ok("Profile updated successfully");
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

        // אדמין — כל המשתמשים הפעילים
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAllActiveUsers();
            return Ok(users);
        }

        // אדמין — כל המשתמשים כולל מבוטלים
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public IActionResult GetAllIncludingInactive()
        {
            var users = _userService.GetAllUsersIncludingInactive();
            return Ok(users);
        }

        // אדמין — פרטים מלאים על משתמש ספציפי
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}/admin")]
        public IActionResult GetByIdForAdmin(int id)
        {
            try
            {
                var user = _userService.GetUserByIdForAdmin(id);
                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
