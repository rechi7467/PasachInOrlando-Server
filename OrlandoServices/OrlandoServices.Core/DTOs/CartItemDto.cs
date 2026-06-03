namespace OrlandoServices.Core.DTOs
{
    public class CartItemDto
    {
        public string CartItemId { get; set; } = null!;
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Dictionary<int, string> FieldValues { get; set; } = new();
    }

    public class UpsertCartItemDto
    {
        public string CartItemId { get; set; } = null!;
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Dictionary<int, string> FieldValues { get; set; } = new();
    }
}
