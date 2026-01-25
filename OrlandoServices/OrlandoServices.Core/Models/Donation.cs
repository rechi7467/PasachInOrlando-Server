using OrlandoServices.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrlandoServices.Core.Models
{
    public class Donation
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }
        public int? OrderId { get; set; }
        public Order? Order { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public PaymentStatus Status { get; set; }

    }
}
