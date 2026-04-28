using OrlandoServices.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.DTOs
{
    public class UpdateServiceFieldDto
    {
        public string? FieldName { get; set; }
        public FieldType? FieldType { get; set; }
        public string? Options { get; set; } // רלוונטי רק ל-Select ו-MultiSelect
        public bool? IsRequired { get; set; }
        public int? OrderIndex { get; set; }
        public bool? IsActive { get; set; }
    }
}
