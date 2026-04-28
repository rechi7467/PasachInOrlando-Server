using System.ComponentModel.DataAnnotations;

namespace OrlandoServices.Core.DTOs
{
    public class CreateDonationDto
    {
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        public int? UserId { get; set; }
        public int? OrderId { get; set; }

        [MaxLength(100)]
        public string? GuestName { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? GuestEmail { get; set; }
    }
}
