using Microsoft.EntityFrameworkCore;
using Subscription_Monitoring_System_Data.Models;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){ }

        public virtual DbSet<Department>? Departments { get; set; }
        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<Client>? Clients { get; set; }
        public virtual DbSet<ServiceDuration>? ServiceDurations { get; set; }
        public virtual DbSet<Service>? Services { get; set; }
        public virtual DbSet<Subscription>? Subscriptions { get; set; }
        public virtual DbSet<Notification>? Notifications { get; set; }
        public virtual DbSet<UserNotification>? UserNotifications { get; set; }
        public virtual DbSet<SubscriptionClient>? SubscriptionClients { get; set; }
        public virtual DbSet<SubscriptionUser>? SubscriptionUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>()
                .HasMany(p => p.SubscriptionClients)
                .WithOne(p => p.Subscription)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subscription>()
                .HasMany(p => p.SubscriptionUsers)
                .WithOne(p => p.Subscription)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(p => p.SubscriptionClients)
                .WithOne(p => p.Client)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(p => p.SubscriptionUsers)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(p => p.UserNotifications)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasMany(p => p.UserNotifications)
                .WithOne(p => p.Notification)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subscription>()
                .HasOne(p => p.SubscriptionHistory)
                .WithMany(p => p.SubscriptionHistories)
                .HasForeignKey(p => p.SubscriptionHistoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Subscription>()
                .HasOne(p => p.Client)
                .WithMany(p => p.InvolvedSubscriptions)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Subscription>()
                .HasOne(p => p.Service)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(p => p.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subscription>()
                .HasOne(p => p.CreatedBy)
                .WithMany(p => p.CreatedSubscriptions)
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Subscription>()
                .HasOne(p => p.UpdatedBy)
                .WithMany(p => p.UpdatedSubscriptions)
                .HasForeignKey(p => p.UpdatedById)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Notification>()
                .HasOne(p => p.Subscription)
                .WithMany(p => p.Notifications)
                .HasForeignKey(p => p.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    ProfilePictureImageName = ImageConstants.DefaultImage,
                    Code = "ADMIN",
                    Password = PasswordHasher.HashPassword("ADMIN"),
                    DepartmentId = 1,
                    IsActive = true
                });

            modelBuilder.Entity<ServiceDuration>().HasData(
                new ServiceDuration
                {
                    Id = 1,
                    Name = "DAILY",
                    Days= 1
                },
                new ServiceDuration
                {
                    Id = 2,
                    Name = "WEEKLY",
                    Days = 7
                },
                new ServiceDuration
                {
                    Id = 3,
                    Name = "MONTHLY",
                    Days = 30
                },
                new ServiceDuration
                {
                    Id = 4,
                    Name = "YEARLY",
                    Days = 365
                });

            modelBuilder.Entity<Department>().HasData(
                new Department
                {
                    Id = 1,
                    Name = "HR DEPARTMENT"
                },
                new Department
                {
                    Id = 2,
                    Name = "ACCOUNTING DEPARTMENT"
                });
        }
    }
}
