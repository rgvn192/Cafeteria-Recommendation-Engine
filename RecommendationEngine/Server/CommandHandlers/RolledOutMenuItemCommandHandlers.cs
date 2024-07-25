using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RecommendationEngine.Common.Exceptions;
using RecommendationEngine.Common.Utils;
using RecommendationEngine.Data.Entities;
using Server.Interface;
using Server.Models.DTO;
using Server.Models.Request;
using Server.Models.Response;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Server.CommandHandlers
{
    public class RolledOutMenuItemCommandHandlers
    {
        private readonly IDailyRolledOutMenuItemService _dailyRolledOutMenuItemService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public RolledOutMenuItemCommandHandlers(INotificationService notificationService, IDailyRolledOutMenuItemService dailyRolledOutMenuItemService, IMapper mapper)
        {
            _dailyRolledOutMenuItemService = dailyRolledOutMenuItemService;
            _notificationService = notificationService;
            _mapper = mapper;

        }
        public async Task<CustomProtocolResponse> RollOutMenuForNextDayForVoting(string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<List<DailyRolledOutMenuItemRequestModel>>(body);

                var dailyRolledOuts = _mapper.Map<List<DailyRolledOutMenuItemRequestModel>, List<DailyRolledOutMenuItem>>(request);
                await _dailyRolledOutMenuItemService.AddRange(dailyRolledOuts);

                var tomorrow = DateTime.UtcNow.Date.AddDays(1);
                List<int> roles = new List<int>()
                    {
                        (int)Roles.User
                    };
                await _notificationService.IssueNotifications(NotificationTypes.MenuItemVoting, $"The Menu for {tomorrow.ToShortDateString()} has been rolled out for voting.", roles);

                var response = new CommonServerResponse
                {
                    Status = "Success",
                    Message = "Daily Menu Rolled out successfully."
                };

                return new CustomProtocolResponse
                {
                    Status = "Success",
                    Body = JsonConvert.SerializeObject(response)
                };
            }
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex.Message)
                };
            }
        }

        public async Task<CustomProtocolResponse> GetRolledOutMenuItemsOfToday(string body)
        {
            try
            {
                List<RolledOutMenuItem> menuItems;

                var today = DateTime.UtcNow.Date;
                Expression<Func<DailyRolledOutMenuItem, bool>> predicate = m => m.CreatedDatetime.Date == today;

                var rolledOutMenuItems = await _dailyRolledOutMenuItemService.GetList<DailyRolledOutMenuItem>(include: $"{nameof(DailyRolledOutMenuItem.MenuItem)}", predicate: predicate);

                menuItems = _mapper.Map<List<DailyRolledOutMenuItem>, List<RolledOutMenuItem>>(rolledOutMenuItems);

                return new CustomProtocolResponse
                {
                    Status = "Success",
                    Body = JsonHelper.SerializeObjectIgnoringCycles(menuItems)
                };
            }
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex.Message)
                };
            }
        }

        public async Task<CustomProtocolResponse> GetRolledOutMenuItemsOfTodayForUser(string body)
        {
            try
            {
                var userId = JsonConvert.DeserializeObject<int>(body);

                List<RolledOutMenuItem> menuItems;

                var rolledOutMenuItems = await _dailyRolledOutMenuItemService.GetRolledOutMenuItemsForUser(userId);

                menuItems = _mapper.Map<List<DailyRolledOutMenuItem>, List<RolledOutMenuItem>>(rolledOutMenuItems);

                return new CustomProtocolResponse
                {
                    Status = "Success",
                    Body = JsonHelper.SerializeObjectIgnoringCycles(menuItems)
                };
            }
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex.Message)
                };
            }
        }

        public async Task<CustomProtocolResponse> ViewVotesOnRolledOutMenuItems(string body)
        {
            try
            {

                List<ViewVotesOnRolledOutMenuItemsResponse> menuItems;

                var today = DateTime.UtcNow.Date;
                Expression<Func<DailyRolledOutMenuItem, bool>> predicate = m => m.CreatedDatetime.Date == today;

                var rolledOutMenuItems = await _dailyRolledOutMenuItemService.GetList<DailyRolledOutMenuItem>(include: $"{nameof(DailyRolledOutMenuItem.MenuItem)}, {nameof(DailyRolledOutMenuItem.DailyRolledOutMenuItemVotes)}", predicate: predicate);

                menuItems = _mapper.Map<List<DailyRolledOutMenuItem>, List<ViewVotesOnRolledOutMenuItemsResponse>>(rolledOutMenuItems);

                return new CustomProtocolResponse
                {
                    Status = "Success",
                    Body = JsonHelper.SerializeObjectIgnoringCycles(menuItems)
                };
            }
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex.Message)
                };
            }
        }

        public async Task<CustomProtocolResponse> ShortListDailyMenuItem(string body)
        {
            try
            {
                var rolledOutMenuItemId = JsonConvert.DeserializeObject<int>(body);

                Expression<Func<DailyRolledOutMenuItem, bool>> predicate = v => v.DailyRolledOutMenuItemId == rolledOutMenuItemId;
                var rolledOutMenuItem = await _dailyRolledOutMenuItemService.GetById<DailyRolledOutMenuItemModel>(rolledOutMenuItemId) ?? throw new AppException("No such rolled out item found");

                rolledOutMenuItem.IsShortListed = true;

                var recommendedMenuItems = await _dailyRolledOutMenuItemService.Update(rolledOutMenuItemId, rolledOutMenuItem);

                var response = new CommonServerResponse
                {
                    Status = "Success",
                    Message = "Successfully Shortlisted Menu Item."
                };

                return new CustomProtocolResponse
                {
                    Status = "Success",
                    Body = JsonConvert.SerializeObject(response)
                };
            }
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex.Message)
                };
            }
        }

        public async Task<CustomProtocolResponse> ViewFinalizedRolledOutMenuItems(string body)
        {
            try
            {
                List<ViewFinalizedRolledOutMenuItemsResponse> finalizedMenuItems = new();

                var today = DateTime.UtcNow.Date;
                Expression<Func<DailyRolledOutMenuItem, bool>> predicate = m => m.CreatedDatetime.Date == today && m.IsShortListed == true;

                var rolledOutMenuItems = await _dailyRolledOutMenuItemService.GetList<DailyRolledOutMenuItem>(include: $"{nameof(DailyRolledOutMenuItem.MenuItem)},{nameof(DailyRolledOutMenuItem.MealType)}", predicate: predicate);

                List<MenuItem> menuItems = new();
                foreach (var menuItem in rolledOutMenuItems)
                {
                    var menuItemModel = _mapper.Map<MenuItem, MenuItemModel>(menuItem.MenuItem);
                    var finalizedMenuItem = new ViewFinalizedRolledOutMenuItemsResponse()
                    {
                        MenuItem = menuItemModel,
                        Mealtype = menuItem.MealType.Name.ToString(),
                    };
                    finalizedMenuItems.Add(finalizedMenuItem);
                }

                return new CustomProtocolResponse
                {
                    Status = "Success",
                    Body = JsonHelper.SerializeObjectIgnoringCycles(finalizedMenuItems)
                };
            }
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex.Message)
                };
            }
        }
    }
}
