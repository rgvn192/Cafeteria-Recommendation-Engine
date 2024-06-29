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

    public class DailyRolledOutMenuItemVoteService : CrudBaseService<DailyRolledOutMenuItemVote>, IDailyRolledOutMenuItemVoteService
    {
        public DailyRolledOutMenuItemVoteService(IDailyRolledOutMenuItemVoteRepository rolledOutMenuItemVoteRepository, IMapper mapper, ILogger<DailyRolledOutMenuItemService> logger) :
            base(rolledOutMenuItemVoteRepository, mapper, logger)
        {
        }

        protected override List<string> ModifiableProperties => new()
        {
            
        };
    }
}
