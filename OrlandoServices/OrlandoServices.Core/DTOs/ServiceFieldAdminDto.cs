using OrlandoServices.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.DTOs
{
    public class ServiceFieldAdminDto
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string FieldName { get; set; } = null!;
        public FieldType FieldType { get; set; }
        public bool IsRequired { get; set; }
        public int OrderIndex { get; set; }
        public string? Options { get; set; }
        public bool IsActive { get; set; }
        public int Version { get; set; }
    }
}
