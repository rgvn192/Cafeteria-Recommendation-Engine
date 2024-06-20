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
                { "AddMenuItem", (body => HandleAddMenuItem(serviceProvider, body), role => role == "Admin") },
                { "UpdateMenuItem", (body => HandleUpdateMenuItem(serviceProvider, body), role => role == "Admin") },
                { "DeleteMenuItem", (body => HandleDeleteMenuItem(serviceProvider, body), role => role == "Admin") },
                { "ToggleMenuItemAvailability", (body => HandleToggleMenuItemAvailability(serviceProvider, body), role => role == "Admin") },
                // Add other commands and their authorization checks here
            };
        }
    }


}
