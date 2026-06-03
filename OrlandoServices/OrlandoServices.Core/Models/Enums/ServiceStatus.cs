namespace OrlandoServices.Core.Models.Enums
{
    public enum ServiceStatus
    {
        Active = 1,       // מוצג ופעיל — ניתן להזמנה
        Unavailable = 2,  // מוצג אבל לא זמין — לא ניתן להזמנה
        Hidden = 3        // מוסתר לחלוטין מהלקוחות
    }
}
