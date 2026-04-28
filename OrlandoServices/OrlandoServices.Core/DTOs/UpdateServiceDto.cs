using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.DTOs
{
    public class UpdateServiceDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? BasePrice { get; set; }
        public bool? IsActive { get; set; }
        public List<CreateServiceFieldDto>? NewFields { get; set; }
        public Dictionary<int, UpdateServiceFieldDto>? UpdatedFields { get; set; }
        public List<int>? FieldIdsToDelete { get; set; }
    }
}
