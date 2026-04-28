namespace OrlandoServices.Core.DTOs
{
    public class OrderFieldValueResponseDto
    {
        public int ServiceFieldId { get; set; }
        public string FieldName { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}
