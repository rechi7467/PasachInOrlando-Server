using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrlandoServices.Core.Models
{
    public class OrderFieldValue
    {
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        [Required]
        public string Value { get; set; } = null!;
        [Required]
        public int ServiceFieldId { get; set; }
        public ServiceField? ServiceField { get; set; }
    }
}
