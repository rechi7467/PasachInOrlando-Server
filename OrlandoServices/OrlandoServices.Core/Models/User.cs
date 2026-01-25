using System.ComponentModel.DataAnnotations;

namespace OrlandoServices.Core.Models
{
    public class User
    {

        [Key] // מציין שזה המפתח הראשי של הטבלה
        public int Id { get; set; }

        [Required] // השדה חייב להיות מלא
        [MaxLength(50)] // מגבלת תווים כדי למנוע נתונים ארוכים מדי
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress] // מוודא שמדובר באימייל תקני
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [Phone] // מוודא שמדובר במספר טלפון תקני
        [MaxLength(20)]
        public string Phone { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string BillingAddress { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string State { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string ZipCode { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // זמן יצירת המשתמש
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Donation> Donations { get; set; } = new List<Donation>();
    }
}
