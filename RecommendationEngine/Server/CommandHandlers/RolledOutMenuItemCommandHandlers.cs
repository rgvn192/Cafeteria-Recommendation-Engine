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
    public static class RolledOutMenuItemCommandHandlers
    {
        public async static Task<CustomProtocolResponse> RollOutMenuForNextDayForVoting(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<List<DailyRolledOutMenuItemRequestModel>>(body);

                using (var scope = serviceProvider.CreateScope())
                {

                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                    var dailyRolledOutMenuItemService = scope.ServiceProvider.GetRequiredService<IDailyRolledOutMenuItemService>();

                    var dailyRolledOuts = mapper.Map<List<DailyRolledOutMenuItemRequestModel>, List<DailyRolledOutMenuItem>>(request);
                    await dailyRolledOutMenuItemService.AddRange(dailyRolledOuts);

                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    var tomorrow = DateTime.UtcNow.Date.AddDays(1);
                    List<int> roles = new List<int>()
                    {
                        (int)Roles.User
                    };
                    await notificationService.IssueNotifications(NotificationTypes.MenuItemVoting, $"The Menu for {tomorrow.ToShortDateString()} has been rolled out for voting.", roles);
                }

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

        public async static Task<CustomProtocolResponse> GetRolledOutMenuItemsOfToday(IServiceProvider serviceProvider, string body)
        {
            try
            {

                List<RolledOutMenuItem> menuItems;

                using (var scope = serviceProvider.CreateScope())
                {
                    var rolledOutMenuItemService = scope.ServiceProvider.GetRequiredService<IDailyRolledOutMenuItemService>();

                    var today = DateTime.UtcNow.Date;
                    Expression<Func<DailyRolledOutMenuItem, bool>> predicate = m => m.CreatedDatetime.Date == today;

                    var rolledOutMenuItems = await rolledOutMenuItemService.GetList<DailyRolledOutMenuItem>(include: $"{nameof(DailyRolledOutMenuItem.MenuItem)}", predicate: predicate);

                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                    menuItems = mapper.Map<List<DailyRolledOutMenuItem>, List<RolledOutMenuItem>>(rolledOutMenuItems);
                }

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

        public async static Task<CustomProtocolResponse> GetRolledOutMenuItemsOfTodayForUser(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var userId = JsonConvert.DeserializeObject<int>(body);

                List<RolledOutMenuItem> menuItems;

                using (var scope = serviceProvider.CreateScope())
                {
                    var rolledOutMenuItemService = scope.ServiceProvider.GetRequiredService<IDailyRolledOutMenuItemService>();

                    var rolledOutMenuItems = await rolledOutMenuItemService.GetRolledOutMenuItemsForUser(userId);

                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                    menuItems = mapper.Map<List<DailyRolledOutMenuItem>, List<RolledOutMenuItem>>(rolledOutMenuItems);
                }

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

        public async static Task<CustomProtocolResponse> ViewVotesOnRolledOutMenuItems(IServiceProvider serviceProvider, string body)
        {
            try
            {

                List<ViewVotesOnRolledOutMenuItemsResponse> menuItems;

                using (var scope = serviceProvider.CreateScope())
                {
                    var rolledOutMenuItemService = scope.ServiceProvider.GetRequiredService<IDailyRolledOutMenuItemService>();

                    var today = DateTime.UtcNow.Date;
                    Expression<Func<DailyRolledOutMenuItem, bool>> predicate = m => m.CreatedDatetime.Date == today;

                    var rolledOutMenuItems = await rolledOutMenuItemService.GetList<DailyRolledOutMenuItem>(include: $"{nameof(DailyRolledOutMenuItem.MenuItem)}, {nameof(DailyRolledOutMenuItem.DailyRolledOutMenuItemVotes)}", predicate: predicate);

                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                    menuItems = mapper.Map<List<DailyRolledOutMenuItem>, List<ViewVotesOnRolledOutMenuItemsResponse>>(rolledOutMenuItems);
                }

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

        public async static Task<CustomProtocolResponse> ShortListDailyMenuItem(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var rolledOutMenuItemId = JsonConvert.DeserializeObject<int>(body);

                using (var scope = serviceProvider.CreateScope())
                {
                    var rolledOutMenuItemService = scope.ServiceProvider.GetRequiredService<IDailyRolledOutMenuItemService>();

                    Expression<Func<DailyRolledOutMenuItem, bool>> predicate = v => v.DailyRolledOutMenuItemId == rolledOutMenuItemId;
                    var rolledOutMenuItem = await rolledOutMenuItemService.GetById<DailyRolledOutMenuItemModel>(rolledOutMenuItemId) ?? throw new AppException("No such rolled out item found");

                    rolledOutMenuItem.IsShortListed = true;

                    var recommendedMenuItems = await rolledOutMenuItemService.Update(rolledOutMenuItemId, rolledOutMenuItem);
                }

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

        public async static Task<CustomProtocolResponse> ViewFinalizedRolledOutMenuItems(IServiceProvider serviceProvider, string body)
        {
            try
            {

                List<ViewFinalizedRolledOutMenuItemsResponse> finalizedMenuItems = new();

                using (var scope = serviceProvider.CreateScope())
                {
                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                    var rolledOutMenuItemService = scope.ServiceProvider.GetRequiredService<IDailyRolledOutMenuItemService>();

                    var today = DateTime.UtcNow.Date;
                    Expression<Func<DailyRolledOutMenuItem, bool>> predicate = m => m.CreatedDatetime.Date == today && m.IsShortListed == true;

                    var rolledOutMenuItems = await rolledOutMenuItemService.GetList<DailyRolledOutMenuItem>(include: $"{nameof(DailyRolledOutMenuItem.MenuItem)},{nameof(DailyRolledOutMenuItem.MealType)}", predicate: predicate);

                    List<MenuItem> menuItems = new();
                    foreach (var menuItem in rolledOutMenuItems)
                    {
                        var menuItemModel = mapper.Map<MenuItem, MenuItemModel>(menuItem.MenuItem);
                        var finalizedMenuItem = new ViewFinalizedRolledOutMenuItemsResponse()
                        {
                            MenuItem = menuItemModel,
                            Mealtype = menuItem.MealType.Name.ToString(),
                        };
                        finalizedMenuItems.Add(finalizedMenuItem);
                    }
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
