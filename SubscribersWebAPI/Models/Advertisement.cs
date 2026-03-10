using System.ComponentModel.DataAnnotations;

namespace SubscribersWebAPI.Models
{
    public class Advertisement
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;
        
        [Range(0, double.MaxValue, ErrorMessage = "Priset måste vara positivt")]
        public decimal ItemPrice { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Annonspriset måste vara positivt")]
        public decimal AdvertisementPrice { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;
        
        // Advertiser information
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
        public string Address { get; set; } = string.Empty;
        
        [Required]
        [StringLength(10)]
        public string PostalCode { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;
        
        // User type and subscription info
        [Required]
        public AdvertiserType AdvertiserType { get; set; }
        
        [StringLength(20)]
        public string? SubscriptionNumber { get; set; }
        
        [StringLength(100)]
        public string? CompanyName { get; set; }
        
        [StringLength(15)]
        public string? OrganizationNumber { get; set; }
        
        // Status and dates
        public AdvertisementStatus Status { get; set; } = AdvertisementStatus.Pending;
        public DateTime PublicationDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
    
    public enum AdvertiserType
    {
        Subscriber = 1,
        Company = 2
    }
    
    public enum AdvertisementStatus
    {
        Pending = 1,
        Published = 2,
        Rejected = 3,
        Expired = 4
    }
}