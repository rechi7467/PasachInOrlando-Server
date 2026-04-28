using System.ComponentModel.DataAnnotations;

namespace OrlandoServices.Core.DTOs
{
    public class CreateGuestOrderDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string BillingAddress { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string State { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string ZipCode { get; set; } = null!;

        public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
    }
}
