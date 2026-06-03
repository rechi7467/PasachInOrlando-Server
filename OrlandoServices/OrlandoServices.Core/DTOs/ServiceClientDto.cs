using OrlandoServices.Core.Models.Enums;

namespace OrlandoServices.Core.DTOs
{
    public class ServiceClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public string? ImageUrl { get; set; }
        public ServiceStatus Status { get; set; }
        public List<ServiceFieldClientDto> Fields { get; set; } = new List<ServiceFieldClientDto>();
    }
}
