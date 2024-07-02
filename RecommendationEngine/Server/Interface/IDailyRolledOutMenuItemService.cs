using RecommendationEngine.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interface
{
    public interface IDailyRolledOutMenuItemService : ICrudBaseService<DailyRolledOutMenuItem>
    {
        Task<List<DailyRolledOutMenuItem>> GetRolledOutMenuItemsForUser(int userId);
    }
}
