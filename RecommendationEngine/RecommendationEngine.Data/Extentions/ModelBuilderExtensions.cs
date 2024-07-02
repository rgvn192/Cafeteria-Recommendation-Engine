using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RecommendationEngine.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Extentions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // Seed data for Role
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, Name = Roles.User },
                new Role { RoleId = 2, Name = Roles.Admin },
                new Role { RoleId = 3, Name = Roles.Chef }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Name = "Rgvn",
                    Email = "rgvn192@gmail.com",
                    RoleId = 2
                },
                new User
                {
                    UserId = 2,
                    Name = "Gordon Ramsay",
                    Email = "gordonramsay@gmail.com",
                    RoleId = 3
                },
                new User
                {
                    UserId = 3,
                    Name = "Amitabh",
                    Email = "amitjilovesbikaji@gmail.com",
                    RoleId = 1
                },
                new User
                {
                    UserId = 4,
                    Name = "Shahrukh",
                    Email = "mainhoondon@gmail.com",
                    RoleId = 1
                },
                new User
                {
                    UserId = 5,
                    Name = "Salman",
                    Email = "bhai@gmail.com",
                    RoleId = 1
                },
                new User
                {
                    UserId = 6,
                    Name = "Leonardo",
                    Email = "onlyunder25@gmail.com",
                    RoleId = 1
                }
            );


            modelBuilder.Entity<MenuItemCategory>().HasData(
                new MenuItemCategory { MenuItemCategoryId = 1, Name = "Snack" },
                new MenuItemCategory { MenuItemCategoryId = 2, Name = "Main Course" },
                new MenuItemCategory { MenuItemCategoryId = 3, Name = "Breads" },
                new MenuItemCategory { MenuItemCategoryId = 4, Name = "Beverages" },
                new MenuItemCategory { MenuItemCategoryId = 5, Name = "Side Dish" }

            );

            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem { MenuItemId = 1, Name = "Poha", Price = 40.00m, MenuItemCategoryId = 1, AverageRating = 4.5m, UserLikeability = 4.7m },
                new MenuItem { MenuItemId = 2, Name = "Upma", Price = 40.00m, MenuItemCategoryId = 1, AverageRating = 4.3m, UserLikeability = 4.6m },
                new MenuItem { MenuItemId = 3, Name = "Fried Idli", Price = 40.00m, MenuItemCategoryId = 1, AverageRating = 4.0m, UserLikeability = 4.2m },

                // Main Courses
                new MenuItem { MenuItemId = 4, Name = "Moong Daal", Price = 40.00m, MenuItemCategoryId = 2, AverageRating = 4.4m, UserLikeability = 4.5m },
                new MenuItem { MenuItemId = 5, Name = "Paneer lababdar", Price = 40.00m, MenuItemCategoryId = 2, AverageRating = 4.7m, UserLikeability = 4.8m },
                new MenuItem { MenuItemId = 6, Name = "Vegetable Biryani", Price = 100.00m, MenuItemCategoryId = 2, AverageRating = 4.8m, UserLikeability = 4.9m },
                new MenuItem { MenuItemId = 7, Name = "Palak Paneer", Price = 90.00m, MenuItemCategoryId = 2, AverageRating = 4.5m, UserLikeability = 4.6m },
                new MenuItem { MenuItemId = 8, Name = "Mix Veg", Price = 90.00m, MenuItemCategoryId = 2, AverageRating = 4.3m, UserLikeability = 4.4m },

                // Adding some low scoring items
                new MenuItem { MenuItemId = 18, Name = "Aloo Gobi", Price = 50.00m, MenuItemCategoryId = 2, AverageRating = 2.5m, UserLikeability = 2.7m },
                new MenuItem { MenuItemId = 19, Name = "Kadhi Pakoda", Price = 60.00m, MenuItemCategoryId = 2, AverageRating = 2.8m, UserLikeability = 3.0m },

                // Breads
                new MenuItem { MenuItemId = 9, Name = "Naan", Price = 15.00m, MenuItemCategoryId = 3, AverageRating = 4.6m, UserLikeability = 4.7m },
                new MenuItem { MenuItemId = 10, Name = "Roti", Price = 10.00m, MenuItemCategoryId = 3, AverageRating = 4.4m, UserLikeability = 4.5m },
                new MenuItem { MenuItemId = 12, Name = "Paratha", Price = 20.00m, MenuItemCategoryId = 3, AverageRating = 4.7m, UserLikeability = 4.8m },

                // Adding some low scoring items
                new MenuItem { MenuItemId = 20, Name = "Butter Naan", Price = 25.00m, MenuItemCategoryId = 3, AverageRating = 3.0m, UserLikeability = 3.2m },
                new MenuItem { MenuItemId = 21, Name = "Plain Paratha", Price = 15.00m, MenuItemCategoryId = 3, AverageRating = 3.3m, UserLikeability = 3.5m },

                // Beverages
                new MenuItem { MenuItemId = 13, Name = "Masala Chai", Price = 15.00m, MenuItemCategoryId = 4, AverageRating = 4.8m, UserLikeability = 4.9m },
                new MenuItem { MenuItemId = 14, Name = "Mango Lassi", Price = 25.00m, MenuItemCategoryId = 4, AverageRating = 4.6m, UserLikeability = 4.7m },

                // Side Dishes
                new MenuItem { MenuItemId = 15, Name = "Cucumber Raita", Price = 20.00m, MenuItemCategoryId = 5, AverageRating = 4.5m, UserLikeability = 4.6m },
                new MenuItem { MenuItemId = 16, Name = "Mixed Vegetable Salad", Price = 30.00m, MenuItemCategoryId = 5, AverageRating = 4.7m, UserLikeability = 4.8m },
                new MenuItem { MenuItemId = 17, Name = "Steamed Basmati Rice", Price = 25.00m, MenuItemCategoryId = 5, AverageRating = 4.3m, UserLikeability = 4.4m },

                // Adding some low scoring items
                new MenuItem { MenuItemId = 22, Name = "Plain Curd", Price = 10.00m, MenuItemCategoryId = 5, AverageRating = 3.5m, UserLikeability = 3.6m },
                new MenuItem { MenuItemId = 23, Name = "Green Salad", Price = 20.00m, MenuItemCategoryId = 5, AverageRating = 3.2m, UserLikeability = 3.4m },

                new MenuItem { MenuItemId = 24, Name = "Butter Chicken", Price = 220.00m, MenuItemCategoryId = 2, AverageRating = 4.2m, UserLikeability = 4.4m },
                new MenuItem { MenuItemId = 25, Name = "Omelette", Price = 100.00m, MenuItemCategoryId = 1, AverageRating = 4.6m, UserLikeability = 4.5m }
            );


            modelBuilder.Entity<NotificationType>().HasData(
                new NotificationType { NotificationTypeId = 1, Name = NotificationTypes.MenuItemVoting },
                new NotificationType { NotificationTypeId = 2, Name = NotificationTypes.FinalizeMenu },
                new NotificationType { NotificationTypeId = 3, Name = NotificationTypes.NewMenuItemAdded },
                new NotificationType { NotificationTypeId = 4, Name = NotificationTypes.MenuItemAvailabilityUpdated },
                new NotificationType { NotificationTypeId = 5, Name = NotificationTypes.DiscardMenuUpdated }

            );

            modelBuilder.Entity<MealType>().HasData(
                new MealType { MealTypeId = 1, Name = MealTypes.Breakfast },
                new MealType { MealTypeId = 2, Name = MealTypes.Lunch },
                new MealType { MealTypeId = 3, Name = MealTypes.Dinner }

            );

            modelBuilder.Entity<Characteristic>().HasData(
                new Characteristic { CharacteristicId = 1, Name = "Veg" },
                new Characteristic { CharacteristicId = 2, Name = "Non-Veg" },
                new Characteristic { CharacteristicId = 3, Name = "Sweet" },
                new Characteristic { CharacteristicId = 4, Name = "Spicy" },
                new Characteristic { CharacteristicId = 5, Name = "Salty" },
                new Characteristic { CharacteristicId = 7, Name = "Gluten-Free" },
                new Characteristic { CharacteristicId = 8, Name = "Dairy-Free" },
                new Characteristic { CharacteristicId = 9, Name = "Nut-Free" },
                new Characteristic { CharacteristicId = 10, Name = "Low-Calorie" },
                new Characteristic { CharacteristicId = 11, Name = "High-Protein" },
                new Characteristic { CharacteristicId = 12, Name = "Vegan" }
            );


            modelBuilder.Entity<MenuItemCharacteristic>().HasData(
                // Breakfast Items
                new MenuItemCharacteristic { Id = 1, MenuItemId = 1, CharacteristicId = 1 }, // Poha - Veg
                new MenuItemCharacteristic { Id = 2, MenuItemId = 1, CharacteristicId = 10 }, // Poha - Low-Calorie
                new MenuItemCharacteristic { Id = 3, MenuItemId = 1, CharacteristicId = 8 }, // Poha - Dairy-Free

                new MenuItemCharacteristic { Id = 4, MenuItemId = 2, CharacteristicId = 1 }, // Upma - Veg
                new MenuItemCharacteristic { Id = 5, MenuItemId = 2, CharacteristicId = 10 }, // Upma - Low-Calorie
                new MenuItemCharacteristic { Id = 6, MenuItemId = 2, CharacteristicId = 5 }, // Upma - salty

                new MenuItemCharacteristic { Id = 7, MenuItemId = 3, CharacteristicId = 1 }, // Fried Idli - Veg
                new MenuItemCharacteristic { Id = 8, MenuItemId = 3, CharacteristicId = 4 }, // Fried Idli - Spicy

                // Main Courses
                new MenuItemCharacteristic { Id = 9, MenuItemId = 4, CharacteristicId = 1 }, // Moong Daal - Veg
                new MenuItemCharacteristic { Id = 10, MenuItemId = 4, CharacteristicId = 11 }, // Moong Daal - High-Protein

                new MenuItemCharacteristic { Id = 11, MenuItemId = 5, CharacteristicId = 1 }, // Paneer lababdar - Veg
                new MenuItemCharacteristic { Id = 12, MenuItemId = 5, CharacteristicId = 11 }, // Paneer lababdar - High-Protein
                new MenuItemCharacteristic { Id = 13, MenuItemId = 5, CharacteristicId = 3 }, // Paneer lababdar - sweet

                new MenuItemCharacteristic { Id = 14, MenuItemId = 6, CharacteristicId = 1 }, // Vegetable Biryani - Veg
                new MenuItemCharacteristic { Id = 15, MenuItemId = 6, CharacteristicId = 4 }, // Vegetable Biryani - Spicy

                new MenuItemCharacteristic { Id = 16, MenuItemId = 7, CharacteristicId = 1 }, // Palak Paneer - Veg
                new MenuItemCharacteristic { Id = 17, MenuItemId = 7, CharacteristicId = 11 }, // Palak Paneer - High-Protein

                new MenuItemCharacteristic { Id = 18, MenuItemId = 8, CharacteristicId = 1 }, // Mix Veg - Veg

                new MenuItemCharacteristic { Id = 19, MenuItemId = 18, CharacteristicId = 1 }, // Aloo Gobi - Veg

                new MenuItemCharacteristic { Id = 20, MenuItemId = 19, CharacteristicId = 1 }, // Kadhi Pakoda - Veg
                new MenuItemCharacteristic { Id = 21, MenuItemId = 19, CharacteristicId = 4 }, // Kadhi Pakoda - spicy

                // Breads
                new MenuItemCharacteristic { Id = 22, MenuItemId = 9, CharacteristicId = 1 }, // Naan - Veg

                new MenuItemCharacteristic { Id = 23, MenuItemId = 10, CharacteristicId = 1 }, // Roti - Veg

                new MenuItemCharacteristic { Id = 24, MenuItemId = 12, CharacteristicId = 1 }, // Paratha - Veg

                new MenuItemCharacteristic { Id = 25, MenuItemId = 20, CharacteristicId = 1 }, // Butter Naan - Veg

                new MenuItemCharacteristic { Id = 26, MenuItemId = 21, CharacteristicId = 1 }, // Plain Paratha - Veg

                // Beverages
                new MenuItemCharacteristic { Id = 27, MenuItemId = 13, CharacteristicId = 1 }, // Masala Chai - Veg

                new MenuItemCharacteristic { Id = 28, MenuItemId = 14, CharacteristicId = 1 }, // Mango Lassi - Veg
                new MenuItemCharacteristic { Id = 29, MenuItemId = 14, CharacteristicId = 3 }, // Mango Lassi - Veg

                // Side Dishes
                new MenuItemCharacteristic { Id = 30, MenuItemId = 15, CharacteristicId = 1 }, // Cucumber Raita - Veg

                new MenuItemCharacteristic { Id = 31, MenuItemId = 16, CharacteristicId = 1 }, // Mixed Vegetable Salad - Veg

                new MenuItemCharacteristic { Id = 32, MenuItemId = 17, CharacteristicId = 1 }, // Steamed Basmati Rice - Veg

                new MenuItemCharacteristic { Id = 33, MenuItemId = 22, CharacteristicId = 1 }, // Plain Curd - Veg

                new MenuItemCharacteristic { Id = 34, MenuItemId = 23, CharacteristicId = 1 },

                // Non - Veg
                new MenuItemCharacteristic { Id = 35, MenuItemId = 24, CharacteristicId = 2 },  // Butter Chicken - Non Veg
                new MenuItemCharacteristic { Id = 36, MenuItemId = 24, CharacteristicId = 11 },  // Butter Chicken - High Protein

                new MenuItemCharacteristic { Id = 37, MenuItemId = 25, CharacteristicId = 11 },  // Omelette - High Protein
                new MenuItemCharacteristic { Id = 38, MenuItemId = 25, CharacteristicId = 2 } // Omelette - Non Veg
            );


            modelBuilder.Entity<UserPreference>().HasData(
                // User 3 Preferences
                new UserPreference { UserPreferenceId = 1, UserId = 3, CharacteristicId = 1 }, // Veg
                new UserPreference { UserPreferenceId = 2, UserId = 3, CharacteristicId = 3 }, // Sweet
                new UserPreference { UserPreferenceId = 3, UserId = 3, CharacteristicId = 8 }, // Dairy Free
                

                // User 4 Preferences
                new UserPreference { UserPreferenceId = 4, UserId = 4, CharacteristicId = 1 }, // Veg
                new UserPreference { UserPreferenceId = 5, UserId = 4, CharacteristicId = 11 }, // High Protein
                new UserPreference { UserPreferenceId = 6, UserId = 4, CharacteristicId = 10 }, // low-calorie

                // User 5 Preferences
                new UserPreference { UserPreferenceId = 7, UserId = 5, CharacteristicId = 2 }, // Non-Veg
                new UserPreference { UserPreferenceId = 8, UserId = 5, CharacteristicId = 11 }, // High Protein


                // User 6 Preferences
                new UserPreference { UserPreferenceId = 9, UserId = 6, CharacteristicId = 1 }, // Veg
                new UserPreference { UserPreferenceId = 10, UserId = 6, CharacteristicId = 4 } // Spicy
            );

        }
    }

}
