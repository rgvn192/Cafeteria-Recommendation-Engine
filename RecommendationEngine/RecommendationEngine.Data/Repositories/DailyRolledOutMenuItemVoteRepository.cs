using Microsoft.Extensions.Logging;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Repositories
{
    
    public class DailyRolledOutMenuItemVoteRepository : CrudBaseRepository<DailyRolledOutMenuItemVote>, IDailyRolledOutMenuItemVoteRepository
    {
        public DailyRolledOutMenuItemVoteRepository(AppDbContext appDbContext, ILogger<DailyRolledOutMenuItemVote> logger) :
            base(appDbContext, logger)
        {

        }

    }
}
