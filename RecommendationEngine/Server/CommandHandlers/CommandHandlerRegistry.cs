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
                { "Login", (body => HandleLogin(serviceProvider, body), role => true) },
                { "AddMenuItem", (body => HandleAddMenuItem(serviceProvider, body), role => role == Roles.Admin.ToString()) },
                { "UpdateMenuItem", (body => HandleUpdateMenuItem(serviceProvider, body), role => role == Roles.Admin.ToString()) },
                { "DeleteMenuItem", (body => HandleDeleteMenuItem(serviceProvider, body), role => role == Roles.Admin.ToString()) },
                { "ToggleMenuItemAvailability", (body => HandleToggleMenuItemAvailability(serviceProvider, body), role => role == Roles.Admin.ToString()) },
                { "GetMenuItems", (body => HandleGetMenuItems(serviceProvider, body), role => role == Roles.Admin.ToString()) },
                { "RollOutMenuForNextDayForVoting", (body => RollOutMenuForNextDayForVoting(serviceProvider, body), role => role == Roles.Chef.ToString()) },
                { "GetRecommendation", (body => GetRecommendation(serviceProvider, body), role => role == Roles.Chef.ToString()) },
                { "ViewVotesOnRolledOutMenuItems", (body => ViewVotesOnRolledOutMenuItems(serviceProvider, body), role => role == Roles.Chef.ToString()) },
                { "GetRolledOutMenuItemsOfToday", (body => GetRolledOutMenuItemsOfToday(serviceProvider, body), role => role == Roles.User.ToString()) },
                { "VoteForDailyMenuItem", (body => VoteForDailyMenuItem(serviceProvider, body), role => role == Roles.User.ToString()) },
                { "ViewFinalizedRolledOutMenuItems", (body => ViewFinalizedRolledOutMenuItems(serviceProvider, body), role => role == Roles.User.ToString()) },
                { "GiveFeedBackOnMenuItem", (body => GiveFeedBackOnMenuItem(serviceProvider, body), role => role == Roles.User.ToString()) },
                { "ShortListDailyMenuItem", (body => ShortListDailyMenuItem(serviceProvider, body), role => role == Roles.Chef.ToString()) },
                // Add other commands and their authorization checks here
            };
        }
    }


}
