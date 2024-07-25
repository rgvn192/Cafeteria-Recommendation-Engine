using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using RecommendationEngine.Common.Exceptions;
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
    public class DiscardedMenuItemService : CrudBaseService<DiscardedMenuItem>, IDiscardedMenuItemService
    {
        private readonly IMenuItemService _menuItemService;
        private readonly INotificationService _notificationService;

        public DiscardedMenuItemService(IDiscardedMenuItemRepository discardedMenuItemRepository, IMapper mapper, ILogger<DiscardedMenuItemService> logger, IMenuItemService menuItemService, INotificationService notificationService) :
            base(discardedMenuItemRepository, mapper, logger)
        {
            _menuItemService = menuItemService;
            _notificationService = notificationService;
        }

        protected override List<string> ModifiableProperties => new()
        {
        };

        public async Task<List<DiscardedMenuItem>> GetDiscardedMenuItemsForCurrentMonth()
        {
            Expression<Func<DiscardedMenuItem, bool>> predicate = m => m.CreatedDatetime.Month == DateTime.UtcNow.Month;
            var discardedMenuItems = await GetList<DiscardedMenuItem>(include : $"{nameof(DiscardedMenuItem.MenuItem)}",predicate: predicate);

            return discardedMenuItems;
        }

        public async Task HandleDiscardedMenuItem(int discardedMenuItemId, bool makeAvailable)
        {
            var discardedMenuItem = await GetById<DiscardedMenuItem>(discardedMenuItemId) ?? throw new AppException("Discarded menu item not found.");
            var menuItem = await _menuItemService.GetById<MenuItem>(discardedMenuItem.MenuItemId) ?? throw new AppException("Menu item not found.");
            if (makeAvailable)
            {
                menuItem.IsAvailable = true;
            }
            else
            {
                menuItem.IsDeleted = true;
            }

            await _menuItemService.Update(menuItem.MenuItemId, menuItem);
        }

        public async Task GenerateDiscardedMenuItemsForThisMonth()
        {
            var lastDiscardedItem = await GetLastDiscardedItem();

            if (lastDiscardedItem != null && lastDiscardedItem.CreatedDatetime.Month == DateTime.UtcNow.Month && lastDiscardedItem.CreatedDatetime.Year == DateTime.UtcNow.Year)
            {
                throw new AppException("Menu items can only be discarded once per month.");
            }

            var lowRatedMenuItems = await GetLowRatedMenuItems();
            if (lowRatedMenuItems == null || lowRatedMenuItems.Count == 0)
            {
                throw new AppException("No menu item to discard");
            }

            if (lowRatedMenuItems.Count > 0)
            {
                await MarkLowRatedMenuItemsAsUnAvailable(lowRatedMenuItems);

                var discardedMenuItems = lowRatedMenuItems.Select(menuItem => new DiscardedMenuItem
                {
                    MenuItemId = menuItem.MenuItemId,
                }).ToList();

                await AddRange(discardedMenuItems);
                List<int> roles = new List<int>()
                    {
                        (int)Roles.User
                    };
                await _notificationService.IssueNotifications(NotificationTypes.DiscardMenuUpdated, $"Discard menu has been updated. Please provide your feedback", roles);
            }
        }

        private async Task<List<MenuItem>> GetLowRatedMenuItems()
        {
            Expression<Func<MenuItem, bool>> predicate = m => m.IsDeleted == false && m.IsAvailable == true && m.AverageRating < 2 && m.UserLikeability < 3;
            var lowRatedMenuItems = await _menuItemService.GetList<MenuItem>(predicate: predicate);

            return lowRatedMenuItems;
        }

        private async Task<DiscardedMenuItem> GetLastDiscardedItem()
        {

            List<string> sort = new() { $"^{nameof(DiscardedMenuItem.CreatedDatetime)}" };
            var lastDiscardedItem = (await GetList<DiscardedMenuItem>(sort: sort)).FirstOrDefault();

            return lastDiscardedItem;
        }

        private async Task MarkLowRatedMenuItemsAsUnAvailable(List<MenuItem> lowRatedMenuItems)
        {
            foreach (var menuItem in lowRatedMenuItems)
            {
                menuItem.IsAvailable = false;
            }
            await _menuItemService.UpdateRange(lowRatedMenuItems);
        }
    }
}
