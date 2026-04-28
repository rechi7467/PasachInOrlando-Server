using OrlandoServices.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrlandoServices.Core.Models
{
    public class OrderFieldValue
    {
        public int Id { get; set; }
        [Required]
        public int OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }
        [Required]
        public string Value { get; set; } = null!;
        [Required]
        public int ServiceFieldId { get; set; }
        public FieldType FieldTypeAtOrderTime { get; set; }
        public string FieldNameAtOrderTime { get; set; } = null!;
        public ServiceField? ServiceField { get; set; }
    }
}
                                                                                                                                                                                    