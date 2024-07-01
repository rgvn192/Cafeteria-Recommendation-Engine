using Microsoft.EntityFrameworkCore;
using RecommendationEngine.Data.Configurations;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuItemCategory> MenuItemCategorys { get; set; }
        public DbSet<DailyRolledOutMenuItem> DailyRolledOutMenuItems { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<MealType> MealTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<DailyRolledOutMenuItemVote> DailyRolledOutMenuItemVotes { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<MenuItemCharacteristic> MenuItemCharacteristicMappings { get; set; }
        public DbSet<UserPreference> UserPreferenceMappings { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new MenuItemConfiguration());
            modelBuilder.ApplyConfiguration(new MenuItemCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new DailyRolledOutMenuItemConfiguration());
            modelBuilder.ApplyConfiguration(new DailyRolledOutMenuItemVoteConfiguration());
            modelBuilder.ApplyConfiguration(new FeedbackConfiguration());
            modelBuilder.ApplyConfiguration(new MealTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new CharacteristicConfiguration());
            modelBuilder.ApplyConfiguration(new UserPreferenceConfiguration());
            modelBuilder.ApplyConfiguration(new MenuItemCharacteristicConfiguration());

            modelBuilder.Seed();
        }
    }
}
