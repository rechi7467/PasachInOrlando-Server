namespace OrlandoServices.Core.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CartItemId { get; set; } = null!;
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string FieldValuesJson { get; set; } = "{}";
        public DateTime CreatedAt { get; set; }
        public User User { get; set; } = null!;
    }
}
