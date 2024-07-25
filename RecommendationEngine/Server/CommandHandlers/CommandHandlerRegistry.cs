using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RecommendationEngine.Data;
using Server.Interface;
using Server.Models.Request;
using Server.Models.Response;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.CommandHandlers.CommandHandlerDelegates;
using RecommendationEngine.Data.Entities;

namespace Server.CommandHandlers
{
    public class CommandHandlerRegistry : ICommandHandlerRegistry
    {
        public Dictionary<string, (CommandHandler Handler, AuthorizationCheck Authorize)> CommandHandlers { get; private set; }

        public CommandHandlerRegistry(IServiceProvider serviceProvider)
        {
            var menuItemHandlers = serviceProvider.GetRequiredService<MenuItemCommandHandlers>();
            var rolledOutMenuItemCommandHandlers = serviceProvider.GetRequiredService<RolledOutMenuItemCommandHandlers>();
            var recommendationCommandHandlers = serviceProvider.GetRequiredService<RecommendationCommandHandlers>();
            var userPreferenceCommandHandlers = serviceProvider.GetRequiredService<UserPreferenceCommandHandlers>();
            var votingCommandHandlers = serviceProvider.GetRequiredService<VotingCommandHandlers>();
            var characteristicCommandHandlers = serviceProvider.GetRequiredService<CharacteristicCommandHandlers>();
            var menuItemFeedbackCommandHandlers = serviceProvider.GetRequiredService<MenuItemFeedbackCommandHandlers>();
            var notificationCommandHandlers = serviceProvider.GetRequiredService<NotificationCommandHandlers>();
            var discardedMenuItemCommandHandlers = serviceProvider.GetRequiredService<DiscardedMenuItemCommandHandlers>();

            CommandHandlers = new Dictionary<string, (CommandHandler, AuthorizationCheck)>
            {
                { "Login", (body => AuthCommandHandlers.Login(serviceProvider, body), role => true) },

                { "AddMenuItem", (body => menuItemHandlers.AddMenuItem(body), role => role == Roles.Admin.ToString()) },
                { "UpdateMenuItem", (body => menuItemHandlers.UpdateMenuItem(body), role => role == Roles.Admin.ToString()) },
                { "DeleteMenuItem", (body => menuItemHandlers.DeleteMenuItem(body), role => role == Roles.Admin.ToString()) },
                { "ToggleMenuItemAvailability", (body => menuItemHandlers.ToggleMenuItemAvailability(body), role => role == Roles.Admin.ToString()) },
                { "GetMenuItems", (body => menuItemHandlers.GetMenuItems(body), role => role == Roles.Admin.ToString() || role == Roles.Chef.ToString()) },

                { "RollOutMenuForNextDayForVoting", (body => rolledOutMenuItemCommandHandlers.RollOutMenuForNextDayForVoting(body), role => role == Roles.Chef.ToString()) },
                { "ViewVotesOnRolledOutMenuItems", (body => rolledOutMenuItemCommandHandlers.ViewVotesOnRolledOutMenuItems(body), role => role == Roles.Chef.ToString()) },
                { "GetRolledOutMenuItemsOfToday", (body => rolledOutMenuItemCommandHandlers.GetRolledOutMenuItemsOfToday(body), role => role == Roles.User.ToString()) },
                { "GetRolledOutMenuItemsOfTodayForUser", (body => rolledOutMenuItemCommandHandlers.GetRolledOutMenuItemsOfTodayForUser(body), role => role == Roles.User.ToString()) },
                { "ViewFinalizedRolledOutMenuItems", (body => rolledOutMenuItemCommandHandlers.ViewFinalizedRolledOutMenuItems(body), role => role == Roles.User.ToString()) },
                { "ShortListDailyMenuItem", (body => rolledOutMenuItemCommandHandlers.ShortListDailyMenuItem(body), role => role == Roles.Chef.ToString()) },

                { "GetRecommendation", (body => recommendationCommandHandlers.GetRecommendation(body), role => role == Roles.Chef.ToString()) },
               
                { "VoteForDailyMenuItem", (body => votingCommandHandlers.VoteForDailyMenuItem(body), role => role == Roles.User.ToString()) },

                { "AddUserPreference", (body => userPreferenceCommandHandlers.AddUserPreference(body), role => role == Roles.User.ToString()) },
                { "DeleteUserPreference", (body => userPreferenceCommandHandlers.DeleteUserPreference(body), role => role == Roles.User.ToString()) },
                { "GetUserPreferences", (body => userPreferenceCommandHandlers.GetUserPreferences(body), role => role == Roles.User.ToString()) },

                { "GetCharacteristics", (body => characteristicCommandHandlers.GetCharacteristics(body), role => role == Roles.User.ToString()) },
                
                { "GiveFeedBackOnMenuItem", (body => menuItemFeedbackCommandHandlers.GiveFeedBackOnMenuItem(body), role => role == Roles.User.ToString()) },
                
                { "GetNotificationsForUser", (body => notificationCommandHandlers.GetNotificationsForUser(body), role => role == Roles.Chef.ToString() || role == Roles.User.ToString() || role == Roles.Admin.ToString())},
                { "IssueNotificationForFinalizedMenu", (body => notificationCommandHandlers.IssueNotificationForFinalizedMenu(body), role => role == Roles.Chef.ToString())},

                { "GenerateDiscardedMenuItems", (body => discardedMenuItemCommandHandlers.GenerateDiscardedMenuItems(body), role => role == Roles.Chef.ToString())},
                { "HandleDiscardedMenuItem", (body => discardedMenuItemCommandHandlers.HandleDiscardedMenuItem(body), role => role == Roles.Chef.ToString())},
                { "GetDiscardedMenuItemsForCurrentMonth", (body => discardedMenuItemCommandHandlers.GetDiscardedMenuItemsForCurrentMonth(body), role => role == Roles.Chef.ToString() || role == Roles.User.ToString() || role == Roles.Admin.ToString())},
                { "GiveFeedBackOnDiscardedMenuItem", (body => discardedMenuItemCommandHandlers.GiveFeedBackOnDiscardedMenuItem(body), role => role == Roles.Chef.ToString() || role == Roles.User.ToString() || role == Roles.Admin.ToString())}
            };
        }
    }


}
