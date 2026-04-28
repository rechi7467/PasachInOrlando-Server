using System.ComponentModel.DataAnnotations;

namespace OrlandoServices.Core.DTOs
{
    public class SetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;
    }
}
