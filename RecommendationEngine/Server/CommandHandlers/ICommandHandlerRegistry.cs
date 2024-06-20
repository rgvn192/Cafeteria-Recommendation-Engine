using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.CommandHandlers
{
    public interface ICommandHandlerRegistry
    {
        Dictionary<string, (CommandHandlerDelegates.CommandHandler Handler, CommandHandlerDelegates.AuthorizationCheck Authorize)> CommandHandlers { get; }
    }

}
