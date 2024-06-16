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
    public class RoleService : CrudBaseService<Role>, IRoleService
    {
        public RoleService(IRoleRepository roleRepository, IMapper mapper, ILogger<Role> logger) :
            base(roleRepository, mapper, logger)
        {
        }

        protected override List<string> ModifiableProperties => new()
        {
            nameof(Role.Name),
        };
    }
}
