using AutoMapper;
using Microsoft.Extensions.Logging;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Interface;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class UserService : CrudBaseService<User>, IUserService
    {
        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<Role> logger) :
            base(userRepository, mapper, logger)
        {
        }

        protected override List<string> ModifiableProperties => new()
        {
            nameof(User.Name),
            nameof(User.IsEnabled),
            nameof(User.LastSeenNotificationAt),
            nameof(User.RoleId)
        };
    }
}
