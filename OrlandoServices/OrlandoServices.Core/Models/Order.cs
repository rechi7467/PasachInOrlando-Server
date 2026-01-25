using OrlandoServices.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrlandoServices.Core.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? user { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
        public OrderStatus Status { get; set; } 
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<OrderFieldValue> OrderFieldValues { get; set; } = new List<OrderFieldValue>();
        public ICollection<Donation> Donations { get; set; } = new List<Donation>();


    }
}
