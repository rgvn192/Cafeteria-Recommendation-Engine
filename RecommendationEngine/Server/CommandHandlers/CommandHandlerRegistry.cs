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
            CommandHandlers = new Dictionary<string, (CommandHandler, AuthorizationCheck)>
            {
                { "Login", (body => AuthCommandHandlers.Login(serviceProvider, body), role => true) },
                { "AddMenuItem", (body => MenuItemCommandHandlers.AddMenuItem(serviceProvider, body), role => role == Roles.Admin.ToString()) },
                { "UpdateMenuItem", (body => MenuItemCommandHandlers.UpdateMenuItem(serviceProvider, body), role => role == Roles.Admin.ToString()) },
                { "DeleteMenuItem", (body => MenuItemCommandHandlers.DeleteMenuItem(serviceProvider, body), role => role == Roles.Admin.ToString()) },
                { "ToggleMenuItemAvailability", (body => MenuItemCommandHandlers.ToggleMenuItemAvailability(serviceProvider, body), role => role == Roles.Admin.ToString()) },
                { "GetMenuItems", (body => MenuItemCommandHandlers.GetMenuItems(serviceProvider, body), role => role == Roles.Admin.ToString() || role == Roles.Chef.ToString()) },
                { "RollOutMenuForNextDayForVoting", (body => RolledOutMenuItemCommandHandlers.RollOutMenuForNextDayForVoting(serviceProvider, body), role => role == Roles.Chef.ToString()) },
                { "GetRecommendation", (body => RecommendationCommandHandlers.GetRecommendation(serviceProvider, body), role => role == Roles.Chef.ToString()) },
                { "ViewVotesOnRolledOutMenuItems", (body => RolledOutMenuItemCommandHandlers.ViewVotesOnRolledOutMenuItems(serviceProvider, body), role => role == Roles.Chef.ToString()) },
                { "GetRolledOutMenuItemsOfToday", (body => RolledOutMenuItemCommandHandlers.GetRolledOutMenuItemsOfToday(serviceProvider, body), role => role == Roles.User.ToString()) },
                { "GetRolledOutMenuItemsOfTodayForUser", (body => RolledOutMenuItemCommandHandlers.GetRolledOutMenuItemsOfTodayForUser(serviceProvider, body), role => role == Roles.User.ToString()) },
                { "VoteForDailyMenuItem", (body => VotingCommandHandlers.VoteForDailyMenuItem(serviceProvider, body), role => role == Roles.User.ToString()) },
                { "AddUserPreference", (body => UserPreferenceCommandHandlers.AddUserPreference(serviceProvider, body), role => role == Roles.User.ToString()) },
                { "DeleteUserPreference", (body => UserPreferenceCommandHandlers.DeleteUserPreference(serviceProvider, body), role => role == Roles.User.ToString()) },
                { "GetUserPreferences", (body => UserPreferenceCommandHandlers.GetUserPreferences(serviceProvider, body), role => role == Roles.User.ToString()) },
                { "GetCharacteristics", (body => CharacteristicCommandHandlers.GetCharacteristics(serviceProvider, body), role => role == Roles.User.ToString()) },
                { "ViewFinalizedRolledOutMenuItems", (body => RolledOutMenuItemCommandHandlers.ViewFinalizedRolledOutMenuItems(serviceProvider, body), role => role == Roles.User.ToString()) },
                { "GiveFeedBackOnMenuItem", (body => MenuItemFeedbackCommandHandlers.GiveFeedBackOnMenuItem(serviceProvider, body), role => role == Roles.User.ToString()) },
                { "ShortListDailyMenuItem", (body => RolledOutMenuItemCommandHandlers.ShortListDailyMenuItem(serviceProvider, body), role => role == Roles.Chef.ToString()) },
                { "GetNotificationsForUser", (body => NotificationCommandHandlers.GetNotificationsForUser(serviceProvider, body), role => role == Roles.Chef.ToString() || role == Roles.User.ToString() || role == Roles.Admin.ToString())},
                { "IssueNotificationForFinalizedMenu", (body => NotificationCommandHandlers.IssueNotificationForFinalizedMenu(serviceProvider, body), role => role == Roles.Chef.ToString())},
                { "GenerateDiscardedMenuItems", (body => DiscardedMenuItemCommandHandlers.GenerateDiscardedMenuItems(serviceProvider, body), role => role == Roles.Chef.ToString())},
                { "HandleDiscardedMenuItem", (body => DiscardedMenuItemCommandHandlers.HandleDiscardedMenuItem(serviceProvider, body), role => role == Roles.Chef.ToString())},
                { "GetDiscardedMenuItemsForCurrentMonth", (body => DiscardedMenuItemCommandHandlers.GetDiscardedMenuItemsForCurrentMonth(serviceProvider, body), role => role == Roles.Chef.ToString() || role == Roles.User.ToString() || role == Roles.Admin.ToString())},
                { "GiveFeedBackOnDiscardedMenuItem", (body => DiscardedMenuItemCommandHandlers.GiveFeedBackOnDiscardedMenuItem(serviceProvider, body), role => role == Roles.Chef.ToString() || role == Roles.User.ToString() || role == Roles.Admin.ToString())}
            };
        }
    }


}
