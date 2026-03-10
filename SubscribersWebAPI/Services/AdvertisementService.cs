using Microsoft.EntityFrameworkCore;
using SubscribersWebAPI.Data;
using SubscribersWebAPI.DTOs;
using SubscribersWebAPI.Models;

namespace SubscribersWebAPI.Services
{
    public interface IAdvertisementService
    {
        Task<AdvertisementDto> CreateAdvertisementAsync(CreateAdvertisementDto createDto);
        Task<IEnumerable<AdvertisementDto>> GetAllAdvertisementsAsync();
        Task<IEnumerable<AdvertisementDto>> GetPublishedAdvertisementsAsync();
        Task<AdvertisementDto?> GetAdvertisementByIdAsync(int id);
        Task<IEnumerable<AdvertisementDto>> GetAdvertisementsBySubscriberAsync(string subscriptionNumber);
        Task<bool> UpdateAdvertisementStatusAsync(int id, Models.AdvertisementStatus status);
        Task<bool> DeleteAdvertisementAsync(int id);
        decimal CalculateAdvertisementPrice(Models.AdvertiserType advertiserType, decimal basePrice = 40);
    }

    public class AdvertisementService : IAdvertisementService
    {
        private readonly SubscriberContext _context;
        private readonly ISubscriberService _subscriberService;

        public AdvertisementService(SubscriberContext context, ISubscriberService subscriberService)
        {
            _context = context;
            _subscriberService = subscriberService;
        }

        public async Task<AdvertisementDto> CreateAdvertisementAsync(CreateAdvertisementDto createDto)
        {
            // Validera prenumerant om det är en prenumerant
            if (createDto.AdvertiserType == Models.AdvertiserType.Subscriber)
            {
                if (string.IsNullOrEmpty(createDto.SubscriptionNumber))
                {
                    throw new ArgumentException("Prenumerationsnummer krävs för prenumeranter");
                }

                var subscriber = await _subscriberService.GetSubscriberByNumberAsync(createDto.SubscriptionNumber);
                if (subscriber == null)
                {
                    throw new ArgumentException($"Ingen aktiv prenumerant hittades med nummer: {createDto.SubscriptionNumber}");
                }

                // Använd prenumerantens uppgifter om de inte är ifyllda
                if (string.IsNullOrEmpty(createDto.FirstName)) createDto.FirstName = subscriber.FirstName;
                if (string.IsNullOrEmpty(createDto.LastName)) createDto.LastName = subscriber.LastName;
                if (string.IsNullOrEmpty(createDto.PhoneNumber)) createDto.PhoneNumber = subscriber.PhoneNumber;
                if (string.IsNullOrEmpty(createDto.Email)) createDto.Email = subscriber.Email;
                if (string.IsNullOrEmpty(createDto.Address)) createDto.Address = subscriber.DeliveryAddress;
                if (string.IsNullOrEmpty(createDto.PostalCode)) createDto.PostalCode = subscriber.PostalCode;
                if (string.IsNullOrEmpty(createDto.City)) createDto.City = subscriber.City;
            }

            // Beräkna annonspris
            var advertisementPrice = CalculateAdvertisementPrice(createDto.AdvertiserType);

            var advertisement = new Advertisement
            {
                Title = createDto.Title,
                Content = createDto.Content,
                ItemPrice = createDto.ItemPrice,
                AdvertisementPrice = advertisementPrice,
                Category = createDto.Category,
                PublicationDate = createDto.PublicationDate,
                
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                PhoneNumber = createDto.PhoneNumber,
                Email = createDto.Email,
                Address = createDto.Address,
                PostalCode = createDto.PostalCode,
                City = createDto.City,
                
                AdvertiserType = createDto.AdvertiserType,
                SubscriptionNumber = createDto.SubscriptionNumber,
                CompanyName = createDto.CompanyName,
                OrganizationNumber = createDto.OrganizationNumber,
                
                Status = Models.AdvertisementStatus.Published, // Auto-publicera för nu
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Advertisements.Add(advertisement);
            await _context.SaveChangesAsync();

            return MapToDto(advertisement);
        }

        public async Task<IEnumerable<AdvertisementDto>> GetAllAdvertisementsAsync()
        {
            var advertisements = await _context.Advertisements
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return advertisements.Select(MapToDto);
        }

        public async Task<IEnumerable<AdvertisementDto>> GetPublishedAdvertisementsAsync()
        {
            var advertisements = await _context.Advertisements
                .Where(a => a.Status == Models.AdvertisementStatus.Published && 
                           a.PublicationDate <= DateTime.UtcNow)
                .OrderByDescending(a => a.PublicationDate)
                .ToListAsync();

            return advertisements.Select(MapToDto);
        }

        public async Task<AdvertisementDto?> GetAdvertisementByIdAsync(int id)
        {
            var advertisement = await _context.Advertisements.FindAsync(id);
            return advertisement != null ? MapToDto(advertisement) : null;
        }

        public async Task<IEnumerable<AdvertisementDto>> GetAdvertisementsBySubscriberAsync(string subscriptionNumber)
        {
            var advertisements = await _context.Advertisements
                .Where(a => a.SubscriptionNumber == subscriptionNumber)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return advertisements.Select(MapToDto);
        }

        public async Task<bool> UpdateAdvertisementStatusAsync(int id, Models.AdvertisementStatus status)
        {
            var advertisement = await _context.Advertisements.FindAsync(id);
            if (advertisement == null)
                return false;

            advertisement.Status = status;
            advertisement.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAdvertisementAsync(int id)
        {
            var advertisement = await _context.Advertisements.FindAsync(id);
            if (advertisement == null)
                return false;

            _context.Advertisements.Remove(advertisement);
            await _context.SaveChangesAsync();
            return true;
        }

        public decimal CalculateAdvertisementPrice(Models.AdvertiserType advertiserType, decimal basePrice = 40)
        {
            return advertiserType switch
            {
                Models.AdvertiserType.Subscriber => 0, // Gratis för prenumeranter
                Models.AdvertiserType.Company => basePrice, // 40 kr för företag
                _ => basePrice
            };
        }

        private static AdvertisementDto MapToDto(Advertisement advertisement)
        {
            return new AdvertisementDto
            {
                Id = advertisement.Id,
                Title = advertisement.Title,
                Content = advertisement.Content,
                ItemPrice = advertisement.ItemPrice,
                AdvertisementPrice = advertisement.AdvertisementPrice,
                Category = advertisement.Category,
                PublicationDate = advertisement.PublicationDate,
                
                FirstName = advertisement.FirstName,
                LastName = advertisement.LastName,
                PhoneNumber = advertisement.PhoneNumber,
                Email = advertisement.Email,
                Address = advertisement.Address,
                PostalCode = advertisement.PostalCode,
                City = advertisement.City,
                
                AdvertiserType = advertisement.AdvertiserType,
                SubscriptionNumber = advertisement.SubscriptionNumber,
                CompanyName = advertisement.CompanyName,
                OrganizationNumber = advertisement.OrganizationNumber,
                
                Status = advertisement.Status,
                CreatedAt = advertisement.CreatedAt
            };
        }
    }
}