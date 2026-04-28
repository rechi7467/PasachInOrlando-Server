using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlandoServices.Core.DTOs
{
    public class UpdateOrderDto
    {
        public List<CreateOrderItemDto>? ItemsToAdd { get; set; }
        public List<int>? ItemIdsToRemove { get; set; }
        public Dictionary<int, UpdateOrderItemDto>? ItemsToUpdate { get; set; }
    }
}
