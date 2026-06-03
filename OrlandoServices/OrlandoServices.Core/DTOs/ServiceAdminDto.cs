using OrlandoServices.Core.Models.Enums;

namespace OrlandoServices.Core.DTOs
{
    public class ServiceAdminDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public string? ImageUrl { get; set; }
        public ServiceStatus Status { get; set; }
        public List<ServiceFieldAdminDto> Fields { get; set; } = new List<ServiceFieldAdminDto>();
    }
}
