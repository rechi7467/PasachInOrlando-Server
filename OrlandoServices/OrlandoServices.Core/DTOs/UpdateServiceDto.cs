using OrlandoServices.Core.Models.Enums;

namespace OrlandoServices.Core.DTOs
{
    public class UpdateServiceDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? BasePrice { get; set; }
        public string? ImageUrl { get; set; }
        public ServiceStatus? Status { get; set; }
        public List<CreateServiceFieldDto>? NewFields { get; set; }
        public Dictionary<int, UpdateServiceFieldDto>? UpdatedFields { get; set; }
        public List<int>? FieldIdsToDelete { get; set; }
    }
}
