using Microsoft.EntityFrameworkCore;
using SubscribersWebAPI.Models;

namespace SubscribersWebAPI.Data
{
    public class SubscriberContext : DbContext
    {
        public SubscriberContext(DbContextOptions<SubscriberContext> options) : base(options)
        {
        }

        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfigurera Subscriber entity
            modelBuilder.Entity<Subscriber>(entity =>
            {
                // Säkerställ att Id är en identity column
                entity.Property(s => s.Id)
                    .ValueGeneratedOnAdd();
                
                // Skapa unik index för prenumerationsnummer
                entity.HasIndex(s => s.SubscriptionNumber)
                    .IsUnique();
            });

            // Konfigurera Advertisement entity
            modelBuilder.Entity<Advertisement>(entity =>
            {
                // Säkerställ att Id är en identity column
                entity.Property(a => a.Id)
                    .ValueGeneratedOnAdd();
                    
                entity.HasIndex(a => a.CreatedAt);
                entity.HasIndex(a => a.PublicationDate);
                entity.HasIndex(a => a.Status);
                entity.HasIndex(a => a.Category);
                
                // Sätt precision för decimal-fält
                entity.Property(a => a.ItemPrice)
                    .HasPrecision(18, 2);
                entity.Property(a => a.AdvertisementPrice)
                    .HasPrecision(18, 2);
            });

            // Seed data för test - Subscribers
            modelBuilder.Entity<Subscriber>().HasData(
                new Subscriber
                {
                    Id = 1,
                    SubscriptionNumber = "SUB001",
                    FirstName = "Anna",
                    LastName = "Andersson",
                    PhoneNumber = "070-123456",
                    Email = "anna.andersson@email.se",
                    DeliveryAddress = "Storgatan 10",
                    PostalCode = "12345",
                    City = "Stockholm",
                    SubscriptionStartDate = DateTime.UtcNow.AddMonths(-6),
                    SubscriptionType = SubscriptionType.Yearly,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddMonths(-6),
                    UpdatedAt = DateTime.UtcNow.AddMonths(-6)
                },
                new Subscriber
                {
                    Id = 2,
                    SubscriptionNumber = "SUB002",
                    FirstName = "Erik",
                    LastName = "Eriksson",
                    PhoneNumber = "070-789012",
                    Email = "erik.eriksson@email.se",
                    DeliveryAddress = "Kungsgatan 25",
                    PostalCode = "41115",
                    City = "Göteborg",
                    SubscriptionStartDate = DateTime.UtcNow.AddMonths(-3),
                    SubscriptionType = SubscriptionType.Monthly,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddMonths(-3),
                    UpdatedAt = DateTime.UtcNow.AddMonths(-3)
                },
                new Subscriber
                {
                    Id = 3,
                    SubscriptionNumber = "SUB003",
                    FirstName = "Maria",
                    LastName = "Johansson",
                    PhoneNumber = "070-345678",
                    Email = "maria.johansson@email.se",
                    DeliveryAddress = "Drottninggatan 5",
                    PostalCode = "21121",
                    City = "Malmö",
                    SubscriptionStartDate = DateTime.UtcNow.AddYears(-1),
                    SubscriptionType = SubscriptionType.Yearly,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow.AddYears(-1),
                    UpdatedAt = DateTime.UtcNow.AddYears(-1)
                }
            );
        }
    }
}