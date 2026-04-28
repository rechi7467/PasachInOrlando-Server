using OrlandoServices.Core.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrlandoServices.Core.Models
{
    public class ServiceField
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ServiceId { get; set; }
        public Services? Service { get; set; }
        [Required]
        [MaxLength(100)]
        public string FieldName { get; set; } = null!;
        [Required]
        public FieldType FieldType { get; set; }
        public string? Options { get; set; } // רלוונטי רק ל-Select ו-MultiSelect
        public int Version { get; set; } = 1;
        public bool IsRequired { get; set; }
        public int OrderIndex { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public ICollection<OrderFieldValue> OrderFieldValues { get; set; } = new List<OrderFieldValue>();
    }
}
