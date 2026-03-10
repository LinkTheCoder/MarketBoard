using SubscribersWebAPI.Models;

namespace SubscribersWebAPI.DTOs
{
    public class CreateAdvertisementDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public decimal ItemPrice { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        
        // Contact information
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        
        // Advertiser type specific fields
        public AdvertiserType AdvertiserType { get; set; }
        public string? SubscriptionNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? OrganizationNumber { get; set; }
    }
    
    public class AdvertisementDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public decimal ItemPrice { get; set; }
        public decimal AdvertisementPrice { get; set; }
        public string Category { get; set; } = string.Empty;
        
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string FullAddress => $"{Address}, {PostalCode} {City}";
        
        public AdvertiserType AdvertiserType { get; set; }
        public string? SubscriptionNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? OrganizationNumber { get; set; }
        
        public AdvertisementStatus Status { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}