using OrlandoServices.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace OrlandoServices.Core.Models
{
    public class Services
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Range(0, double.MaxValue)]
        public decimal BasePrice { get; set; }
        public ServiceStatus Status { get; set; } = ServiceStatus.Active;
        public string? ImageUrl { get; set; }
        public int SortOrder { get; set; } = 0;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<ServiceField> ServiceFields { get; set; } = new List<ServiceField>();
    }
}
