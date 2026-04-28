namespace OrlandoServices.Core.DTOs
{
    public class OrderItemResponseDto
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderFieldValueResponseDto> FieldValues { get; set; } = new List<OrderFieldValueResponseDto>();
    }
}
