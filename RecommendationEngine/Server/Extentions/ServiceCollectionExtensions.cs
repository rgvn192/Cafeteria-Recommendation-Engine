using Microsoft.Extensions.DependencyInjection;
using Server.Interface;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthorisationService, AuthorisationService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<IDailyRolledOutMenuItemService, DailyRolledOutMenuItemService>();
            services.AddScoped<IDailyRolledOutMenuItemVoteService, DailyRolledOutMenuItemVoteService>();
            services.AddScoped<IFeedbackService, FeedBackService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IDiscardedMenuItemService, DiscardedMenuItemService>();
            services.AddScoped<IDiscardedMenuItemFeedbackService, DiscardedMenuItemFeedbackService>();
            services.AddScoped<ISentimentAnalyzer, SentimentAnalyzer>();
            services.AddScoped<IKeywordExtractor, KeywordExtractor>();
            services.AddScoped<IUserPreferenceService, UserPreferenceService>();
            services.AddScoped<ICharacteristicService, CharacteristicService>();
            return services;
        }
    }
}
