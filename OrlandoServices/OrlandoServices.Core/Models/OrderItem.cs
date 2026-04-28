using System.ComponentModel.DataAnnotations;

namespace OrlandoServices.Core.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        [Required]
        public int ServiceId { get; set; }
        public Services? Service { get; set; }

        [Required]
        [MaxLength(100)]
        public string ServiceName { get; set; } = null!;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount { get; set; } = 0;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }

        public ICollection<OrderFieldValue> OrderFieldValues { get; set; } = new List<OrderFieldValue>();
    }
}
