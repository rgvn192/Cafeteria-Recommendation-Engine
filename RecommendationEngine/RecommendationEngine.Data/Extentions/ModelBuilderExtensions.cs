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

            modelBuilder.Entity<MenuItemCategory>().HasData(
                new MenuItemCategory { MenuItemCategoryId = 1, Name = "Snack" },
                new MenuItemCategory { MenuItemCategoryId = 2, Name = "Main Course" },
                new MenuItemCategory { MenuItemCategoryId = 3, Name = "Breads" },
                new MenuItemCategory { MenuItemCategoryId = 4, Name = "Beverages" },
                new MenuItemCategory { MenuItemCategoryId = 5, Name = "Side Dish" }

            );

            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem { MenuItemId = 1, Name = "Poha", Price = 40.00m, MenuItemCategoryId = 1 },
                new MenuItem { MenuItemId = 2, Name = "Upma", Price = 40.00m, MenuItemCategoryId = 1 },
                new MenuItem { MenuItemId = 3, Name = "Fried Idli", Price = 40.00m, MenuItemCategoryId = 1 },

                // Main Courses
                new MenuItem { MenuItemId = 4, Name = "Moong Daal", Price = 40.00m, MenuItemCategoryId = 2 },
                new MenuItem { MenuItemId = 5, Name = "Paneer lababdar", Price = 40.00m, MenuItemCategoryId = 2 },
                new MenuItem { MenuItemId = 6, Name = "Vegetable Biryani", Price = 100.00m, MenuItemCategoryId = 2 },
                new MenuItem { MenuItemId = 7, Name = "Palak Paneer", Price = 90.00m, MenuItemCategoryId = 2 },
                new MenuItem { MenuItemId = 8, Name = "Mix Veg", Price = 90.00m, MenuItemCategoryId = 2 },

                // Breads
                new MenuItem { MenuItemId = 9, Name = "Naan", Price = 15.00m, MenuItemCategoryId = 3 },
                new MenuItem { MenuItemId = 10, Name = "Roti", Price = 10.00m, MenuItemCategoryId = 3 },
                new MenuItem { MenuItemId = 12, Name = "Paratha", Price = 20.00m, MenuItemCategoryId = 3 },

                // Beverages
                new MenuItem { MenuItemId = 13, Name = "Masala Chai", Price = 15.00m, MenuItemCategoryId = 4 },
                new MenuItem { MenuItemId = 14, Name = "Mango Lassi", Price = 25.00m, MenuItemCategoryId = 4 },

                // Side Dishes
                new MenuItem { MenuItemId = 15, Name = "Cucumber Raita", Price = 20.00m, MenuItemCategoryId = 5 },
                new MenuItem { MenuItemId = 16, Name = "Mixed Vegetable Salad", Price = 30.00m, MenuItemCategoryId = 5 },
                new MenuItem { MenuItemId = 17, Name = "Steamed Basmati Rice", Price = 25.00m, MenuItemCategoryId = 5 }

            );
        }


    }

}
