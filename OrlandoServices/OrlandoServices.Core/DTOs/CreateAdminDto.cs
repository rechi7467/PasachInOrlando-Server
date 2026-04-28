using System.ComponentModel.DataAnnotations;

namespace OrlandoServices.Core.DTOs
{
    public class CreateAdminDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;
    }
}
