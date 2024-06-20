using Microsoft.Extensions.DependencyInjection;
using RecommendationEngine.Data.Interface;
using RecommendationEngine.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Extentions
{
    public static class ServiceCollectionRepositoryExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            return services;
        }
    }
}
