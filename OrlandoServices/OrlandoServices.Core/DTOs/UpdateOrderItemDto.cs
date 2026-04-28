namespace OrlandoServices.Core.DTOs
{
    public class UpdateOrderItemDto
    {
        public int? Quantity { get; set; }
        public Dictionary<int, string>? FieldValues { get; set; }
    }
}
