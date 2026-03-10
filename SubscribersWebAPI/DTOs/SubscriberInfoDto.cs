using System.ComponentModel.DataAnnotations;
using SubscribersWebAPI.Models;

namespace SubscribersWebAPI.DTOs
{
    public class SubscriberInfoDto
    {
        public string SubscriptionNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string FullAddress => $"{DeliveryAddress}, {PostalCode} {City}";
        public bool IsActive { get; set; }
    }

    public class CreateSubscriberDto
    {
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

        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }

        [Required]
        public SubscriptionType SubscriptionType { get; set; }

        public bool IsActive { get; set; } = true;

        public Subscriber ToSubscriber()
        {
            return new Subscriber
            {
                SubscriptionNumber = this.SubscriptionNumber,
                FirstName = this.FirstName,
                LastName = this.LastName,
                PhoneNumber = this.PhoneNumber,
                Email = this.Email,
                DeliveryAddress = this.DeliveryAddress,
                PostalCode = this.PostalCode,
                City = this.City,
                SubscriptionStartDate = this.SubscriptionStartDate ?? DateTime.UtcNow,
                SubscriptionEndDate = this.SubscriptionEndDate,
                SubscriptionType = this.SubscriptionType,
                IsActive = this.IsActive
            };
        }
    }
}