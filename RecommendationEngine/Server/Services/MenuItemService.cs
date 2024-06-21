using AutoMapper;
using Microsoft.Extensions.Logging;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Interface;
using RecommendationEngine.Data.Repositories;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{

    public class MenuItemService : CrudBaseService<MenuItem>, IMenuItemService
    {
        private readonly IDailyRolledOutMenuItemService _dailyRolledOutMenuItemService;

        public MenuItemService(IMenuItemRepository menuItemRepository, IMapper mapper, ILogger<MenuItemService> logger, IDailyRolledOutMenuItemService dailyRolledOutMenuItemService) :
            base(menuItemRepository, mapper, logger)
        {
            _dailyRolledOutMenuItemService = dailyRolledOutMenuItemService;
        }

        protected override List<string> ModifiableProperties => new()
        {
            nameof(MenuItem.Name),
            nameof(MenuItem.IsAvailable),
            nameof(MenuItem.Price),
            nameof(MenuItem.UserLikeability),
            nameof(MenuItem.AverageRating),
            nameof(MenuItem.Comments),
        };

        public async Task<List<MenuItem>> GetRecommendationByMenuItemCategory(int menuItemCategory, int limit)
        {
            var menuItems = await GetMenuItemsByCategory(menuItemCategory);
            var yesterdayRolledOutShorlistedMenuItems = await GetYesterdayRolledOutMenuItems();

            var filteredMenuItems = FilterOutShortlistedYesterday(menuItems, yesterdayRolledOutShorlistedMenuItems);

            var menuItemScores = CalculateCombinedScores(filteredMenuItems);

            var topMenuItems = GetTopMenuItems(menuItemScores, limit);

            return topMenuItems;
        }

        private async Task<List<DailyRolledOutMenuItem>> GetYesterdayRolledOutMenuItems()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1).Date;

            Expression<Func<DailyRolledOutMenuItem, bool>> predicate = d => d.IsShortListed == true && d.CreatedDatetime.Date == yesterday;
            return await _dailyRolledOutMenuItemService.GetList<DailyRolledOutMenuItem>(predicate: predicate);
        }

        private async Task<List<MenuItem>> GetMenuItemsByCategory(int menuItemCategory)
        {
            Expression<Func<MenuItem, bool>> predicate = m => m.MenuItemCategoryId == menuItemCategory && m.IsAvailable == true;
            return await GetList<MenuItem>(predicate: predicate);
        }

        private List<MenuItem> FilterOutShortlistedYesterday(List<MenuItem> menuItems, List<DailyRolledOutMenuItem> dailyRolledOutMenuItems)
        {
            var menuItemIdsShortlistedYesterday = dailyRolledOutMenuItems
                .Select(dri => dri.MenuItemId)
                .ToHashSet();

            return menuItems.Where(mi => !menuItemIdsShortlistedYesterday.Contains(mi.MenuItemId)).ToList();
        }

        private Dictionary<MenuItem, decimal> CalculateCombinedScores(List<MenuItem> menuItems)
        {
            const decimal averageRatingWeight = 0.6m;
            const decimal userLikeabilityWeight = 0.4m;

            return menuItems.ToDictionary(
                menuItem => menuItem,
                menuItem => (menuItem.AverageRating * averageRatingWeight) +
                            (menuItem.UserLikeability * userLikeabilityWeight)
            );
        }

        private List<MenuItem> GetTopMenuItems(Dictionary<MenuItem, decimal> menuItemScores, int limit)
        {
            return menuItemScores.OrderByDescending(pair => pair.Value)
                                 .Take(limit)
                                 .Select(pair => pair.Key)
                                 .ToList();
        }

    }
}
