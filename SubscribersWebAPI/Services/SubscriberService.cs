using Microsoft.EntityFrameworkCore;
using SubscribersWebAPI.Data;
using SubscribersWebAPI.DTOs;
using SubscribersWebAPI.Models;

namespace SubscribersWebAPI.Services
{
    public interface ISubscriberService
    {
        Task<SubscriberInfoDto?> GetSubscriberByNumberAsync(string subscriptionNumber);
        Task<IEnumerable<SubscriberInfoDto>> GetAllActiveSubscribersAsync();
        Task<Subscriber?> GetSubscriberByIdAsync(int id);
        Task<Subscriber> CreateSubscriberAsync(Subscriber subscriber);
        Task<Subscriber?> UpdateSubscriberAsync(int id, Subscriber subscriber);
        Task<bool> DeleteSubscriberAsync(int id);
        Task<bool> SubscriberExistsAsync(string subscriptionNumber);
    }

    public class SubscriberService : ISubscriberService
    {
        private readonly SubscriberContext _context;

        public SubscriberService(SubscriberContext context)
        {
            _context = context;
        }

        public async Task<SubscriberInfoDto?> GetSubscriberByNumberAsync(string subscriptionNumber)
        {
            var subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(s => s.SubscriptionNumber == subscriptionNumber && s.IsActive);

            if (subscriber == null)
                return null;

            return new SubscriberInfoDto
            {
                SubscriptionNumber = subscriber.SubscriptionNumber,
                FirstName = subscriber.FirstName,
                LastName = subscriber.LastName,
                PhoneNumber = subscriber.PhoneNumber,
                Email = subscriber.Email,
                DeliveryAddress = subscriber.DeliveryAddress,
                PostalCode = subscriber.PostalCode,
                City = subscriber.City,
                IsActive = subscriber.IsActive
            };
        }

        public async Task<IEnumerable<SubscriberInfoDto>> GetAllActiveSubscribersAsync()
        {
            var subscribers = await _context.Subscribers
                .Where(s => s.IsActive)
                .Select(s => new SubscriberInfoDto
                {
                    SubscriptionNumber = s.SubscriptionNumber,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    PhoneNumber = s.PhoneNumber,
                    Email = s.Email,
                    DeliveryAddress = s.DeliveryAddress,
                    PostalCode = s.PostalCode,
                    City = s.City,
                    IsActive = s.IsActive
                })
                .ToListAsync();

            return subscribers;
        }

        public async Task<Subscriber?> GetSubscriberByIdAsync(int id)
        {
            return await _context.Subscribers.FindAsync(id);
        }

        public async Task<Subscriber> CreateSubscriberAsync(Subscriber subscriber)
        {
            subscriber.CreatedAt = DateTime.UtcNow;
            subscriber.UpdatedAt = DateTime.UtcNow;
            
            _context.Subscribers.Add(subscriber);
            await _context.SaveChangesAsync();
            return subscriber;
        }

        public async Task<Subscriber?> UpdateSubscriberAsync(int id, Subscriber subscriber)
        {
            var existingSubscriber = await _context.Subscribers.FindAsync(id);
            if (existingSubscriber == null)
                return null;

            existingSubscriber.FirstName = subscriber.FirstName;
            existingSubscriber.LastName = subscriber.LastName;
            existingSubscriber.PhoneNumber = subscriber.PhoneNumber;
            existingSubscriber.Email = subscriber.Email;
            existingSubscriber.DeliveryAddress = subscriber.DeliveryAddress;
            existingSubscriber.PostalCode = subscriber.PostalCode;
            existingSubscriber.City = subscriber.City;
            existingSubscriber.SubscriptionType = subscriber.SubscriptionType;
            existingSubscriber.SubscriptionEndDate = subscriber.SubscriptionEndDate;
            existingSubscriber.IsActive = subscriber.IsActive;
            existingSubscriber.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingSubscriber;
        }

        public async Task<bool> DeleteSubscriberAsync(int id)
        {
            var subscriber = await _context.Subscribers.FindAsync(id);
            if (subscriber == null)
                return false;

            // Mjuk borttagning - sätt IsActive till false istället för att ta bort från databasen
            subscriber.IsActive = false;
            subscriber.UpdatedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SubscriberExistsAsync(string subscriptionNumber)
        {
            return await _context.Subscribers
                .AnyAsync(s => s.SubscriptionNumber == subscriptionNumber);
        }
    }
}