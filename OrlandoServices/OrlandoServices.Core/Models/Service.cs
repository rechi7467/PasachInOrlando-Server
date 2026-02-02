using System.ComponentModel.DataAnnotations;

namespace OrlandoServices.Core.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Range(0, double.MaxValue)]
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<ServiceField> ServiceFields { get; set; } = new List<ServiceField>();


    }
}
