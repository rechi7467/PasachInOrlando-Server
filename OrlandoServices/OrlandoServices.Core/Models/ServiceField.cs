using Microsoft.VisualBasic.FileIO;
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
        public Service? Service { get; set; }
        [Required]
        public string FieldName { get; set; } = null!;
        [Required]
        public FieldType FieldType { get; set; }
        public bool IsRequired { get; set; }
        public int OrderIndex { get; set; }
        public ICollection<OrderFieldValue> OrderFieldValues { get; set; } = new List<OrderFieldValue>();
    }
}
