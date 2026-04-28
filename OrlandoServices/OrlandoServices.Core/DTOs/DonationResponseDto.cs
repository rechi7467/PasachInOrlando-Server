using OrlandoServices.Core.Models.Enums;

namespace OrlandoServices.Core.DTOs
{
    public class DonationResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UserId { get; set; }
        public int? OrderId { get; set; }
        public string? GuestName { get; set; }
        public string? GuestEmail { get; set; }
    }
}
