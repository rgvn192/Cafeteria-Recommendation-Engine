using AutoMapper;
using Microsoft.Extensions.Logging;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Interface;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class DailyRolledOutMenuItemService : CrudBaseService<DailyRolledOutMenuItem>, IDailyRolledOutMenuItemService
    {

        private readonly IUserService _userService;

        public DailyRolledOutMenuItemService(IUserService userService, IDailyRolledOutMenuItemRepository dailyRolledOutMenuItemRepository, IMapper mapper, ILogger<DailyRolledOutMenuItemService> logger) :
            base(dailyRolledOutMenuItemRepository, mapper, logger)
        {
            _userService = userService;
        }

        protected override List<string> ModifiableProperties => new()
        {
            nameof(DailyRolledOutMenuItem.IsShortListed),
        };

        public async Task<List<DailyRolledOutMenuItem>> GetRolledOutMenuItemsForUser(int userId)
        {
            var user = await _userService.GetById<User>(userId, $"{nameof(User.Preferences)}");
            var userPreferences = user.Preferences;
            var today = DateTime.UtcNow.Date;

            Expression<Func<DailyRolledOutMenuItem, bool>> predicate = m => m.CreatedDatetime.Date == today;

            var rolledOutMenuItems = await GetList<DailyRolledOutMenuItem>(include: $"{nameof(DailyRolledOutMenuItem.MenuItem)}.{nameof(MenuItem.Characteristics)}, {nameof(DailyRolledOutMenuItem.MealType)}", predicate: predicate);

            var sortedRolledOutMenuItems = SortRolledOutMenuItemsByUserPreference(rolledOutMenuItems, userPreferences);
            return sortedRolledOutMenuItems;
        }

        private List<DailyRolledOutMenuItem> SortRolledOutMenuItemsByUserPreference(List<DailyRolledOutMenuItem> rolledOutMenuItems, List<UserPreference> userPreferences)
        {
            var userPreferenceIds = userPreferences.Select(up => up.CharacteristicId).ToList();

            var sortedRolledOutMenuItems = rolledOutMenuItems
                .OrderByDescending(item => item.MenuItem.Characteristics.Count(mc => userPreferenceIds.Contains(mc.CharacteristicId)))
                .ToList();

            return sortedRolledOutMenuItems;
        }
    }
}
