namespace OrlandoServices.Core.DTOs
{
    public class CreateOrderItemDto
    {
        public int ServiceId { get; set; }
        public int Quantity { get; set; } = 1;
        public Dictionary<int, string> FieldValues { get; set; } = new Dictionary<int, string>();
    }
}
