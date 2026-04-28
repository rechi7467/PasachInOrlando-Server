using OrlandoServices.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.DTOs
{
    public class CreateServiceFieldDto
    {
        public int ServiceId { get; set; }
        public string FieldName { get; set; } = null!;
        public string? Options { get; set; } // רלוונטי רק ל-Select ו-MultiSelect
        public FieldType FieldType { get; set; }
        public bool IsRequired { get; set; }
        public int OrderIndex { get; set; }
    }
}
