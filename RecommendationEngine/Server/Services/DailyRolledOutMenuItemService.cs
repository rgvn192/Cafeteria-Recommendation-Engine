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
    public class DailyRolledOutMenuItemService : CrudBaseService<DailyRolledOutMenuItem>, IDailyRolledOutMenuItemService
    {
        public DailyRolledOutMenuItemService(IDailyRolledOutMenuItemRepository dailyRolledOutMenuItemRepository, IMapper mapper, ILogger<DailyRolledOutMenuItemService> logger) :
            base(dailyRolledOutMenuItemRepository, mapper, logger)
        {
        }

        protected override List<string> ModifiableProperties => new()
        {
            nameof(DailyRolledOutMenuItem.IsShortListed),
        };
    }
}
