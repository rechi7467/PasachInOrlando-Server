using System.ComponentModel.DataAnnotations;

namespace OrlandoServices.Core.DTOs
{
    public class AdminLoginDto
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
