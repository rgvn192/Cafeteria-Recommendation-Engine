using Microsoft.EntityFrameworkCore;
using RecommendationEngine.Data.Entities;
using System;
using System.Collections.Generic;
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
                new Role { RoleId = 1, Name = Roles.User},
                new Role { RoleId = 2, Name = Roles.Admin},
                new Role { RoleId = 3, Name = Roles.Chef}
            );

        }
    }

}
