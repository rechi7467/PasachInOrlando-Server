using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.DTOs
{
    public class CreateOrderFieldValueDto
    {
        public int ServiceFieldId { get; set; }
        public string Value { get; set; } = null!;
    }
}
