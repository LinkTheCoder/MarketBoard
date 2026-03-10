using System.ComponentModel.DataAnnotations;

namespace SubscribersWebAPI.Models
{
    public class Subscriber
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(20)]
        public string SubscriptionNumber { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string DeliveryAddress { get; set; } = string.Empty;
        
        [Required]
        [StringLength(10)]
        public string PostalCode { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;
        
        public DateTime SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        
        [Required]
        public SubscriptionType SubscriptionType { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
    
    public enum SubscriptionType
    {
        Monthly = 1,
        Quarterly = 2,
        HalfYearly = 3,
        Yearly = 4
    }
}