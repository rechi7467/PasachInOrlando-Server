using OrlandoServices.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrlandoServices.Core.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public PaymentMethods PaymentMethod { get; set; }

        [Required]
        [MaxLength(100)]
        public string TransactionReference { get; set; } = null!;

        [Required]
        public PaymentStatus Status { get; set; } 
        public DateTime? PaidAt { get; set; }
    }
}
