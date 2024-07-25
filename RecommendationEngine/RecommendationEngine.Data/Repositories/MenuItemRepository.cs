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
    public class MenuItemRepository : CrudBaseRepository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(AppDbContext appDbContext, ILogger<MenuItemRepository> logger) :
            base(appDbContext, logger)
        {

        }

    }
}
