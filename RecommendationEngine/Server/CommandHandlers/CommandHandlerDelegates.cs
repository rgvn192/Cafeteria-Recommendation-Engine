using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RecommendationEngine.Data;
using Server.Interface;
using Server.Models.Request;
using Server.Models.Response;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Models.DTO;
using AutoMapper;
using RecommendationEngine.Data.Entities;
using System.Linq.Expressions;
using RecommendationEngine.Common.Exceptions;
using RecommendationEngine.Common.Utils;

namespace Server.CommandHandlers
{
    public static class CommandHandlerDelegates
    {
        public delegate Task<CustomProtocolResponse> CommandHandler(string body);
        public delegate bool AuthorizationCheck(string role);

        public static async Task<CustomProtocolResponse> HandleLogin(IServiceProvider serviceProvider, string body)
        {
            var loginRequest = JsonConvert.DeserializeObject<LoginRequestModel>(body);
            var authService = serviceProvider.GetRequiredService<IAuthorisationService>();
            var user = await authService.AuthenticateUser(loginRequest);

            var loginResponse = new LoginResponseModel
            {
                Role = user?.Role.Name.ToString(),
                UserId = user?.UserId
            };

            return new CustomProtocolResponse
            {
                Status = user != null ? "Success" : "Failed",
                Body = JsonConvert.SerializeObject(loginResponse)
            };
        }

        public async static Task<CustomProtocolResponse> HandleAddMenuItem(IServiceProvider serviceProvider, string body)
        {
            var request = JsonConvert.DeserializeObject<MenuItemModel>(body);

            using (var scope = serviceProvider.CreateScope())
            {
                var menuItemService = scope.ServiceProvider.GetRequiredService<IMenuItemService>();

                await menuItemService.Add(request);

                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                List<int> roles = new()
                    {
                        (int)Roles.User,
                        (int)Roles.Chef
                    };
                await notificationService.IssueNotifications(NotificationTypes.NewMenuItemAdded, $"{request?.Name} has been added to Menu", roles);
            }

            var response = new AddMenuItemResponseModel
            {
                Status = "Success",
                Message = "Menu item added successfully."
            };

            return new CustomProtocolResponse
            {
                Status = "Success",
                Body = JsonConvert.SerializeObject(response)
            };
        }

        public async static Task<CustomProtocolResponse> HandleUpdateMenuItem(IServiceProvider serviceProvider, string body)
        {
            var request = JsonConvert.DeserializeObject<UpdateMenuItemRequestModel>(body);

            using (var scope = serviceProvider.CreateScope())
            {
                var menuItemService = scope.ServiceProvider.GetRequiredService<IMenuItemService>();

                var menuItem = await menuItemService.GetById<MenuItemModel>(request.MenuItemId);

                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                menuItem = mapper.Map(request, menuItem);

                await menuItemService.Update(request.MenuItemId, menuItem);
            }

            var response = new UpdateMenuItemResponseModel
            {
                Status = "Success",
                Message = "Menu item updated successfully."
            };

            return new CustomProtocolResponse
            {
                Status = "Success",
                Body = JsonConvert.SerializeObject(response)
            };
        }

        public async static Task<CustomProtocolResponse> HandleDeleteMenuItem(IServiceProvider serviceProvider, string body)
        {
            var request = JsonConvert.DeserializeObject<int>(body);

            using (var scope = serviceProvider.CreateScope())
            {
                var menuItemService = scope.ServiceProvider.GetRequiredService<IMenuItemService>();

                await menuItemService.Delete(request);
            }

            var response = new DeleteMenuResponseModel
            {
                Status = "Success",
                Message = "Menu item deleted successfully."
            };

            return new CustomProtocolResponse
            {
                Status = "Success",
                Body = JsonConvert.SerializeObject(response)
            };
        }

        public async static Task<CustomProtocolResponse> HandleGetMenuItems(IServiceProvider serviceProvider, string body)
        {
            var request = JsonConvert.DeserializeObject<GetMenuItemsRequestModel>(body);

            List<MenuItemModel> menuItems;

            using (var scope = serviceProvider.CreateScope())
            {
                var menuItemService = scope.ServiceProvider.GetRequiredService<IMenuItemService>();

                menuItems = await menuItemService.GetList<MenuItemModel>(null, null, null, request.Limit, request.Offset);
            }

            var response = new GetMenuItemsResponseModel
            {
                MenuItems = menuItems
            };

            return new CustomProtocolResponse
            {
                Status = "Success",
                Body = JsonConvert.SerializeObject(response)
            };
        }

        public async static Task<CustomProtocolResponse> HandleToggleMenuItemAvailability(IServiceProvider serviceProvider, string body)
        {
            var request = JsonConvert.DeserializeObject<int>(body);

            using (var scope = serviceProvider.CreateScope())
            {
                var menuItemService = scope.ServiceProvider.GetRequiredService<IMenuItemService>();
                var menuItem = await menuItemService.GetById<MenuItemModel>(request) ?? throw new AppException(ErrorResponse.ErrorEnum.NotFound,
                    LogExtensions.GetLogMessage(nameof(HandleToggleMenuItemAvailability), null, "No such Menu Item found"));

                menuItem.IsAvailable = !menuItem.IsAvailable;
                await menuItemService.Update(menuItem.MenuItemId, menuItem);

                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                List<int> roles = new()
                    {
                        (int)Roles.User,
                        (int)Roles.Chef
                    };
                string availability = menuItem.IsAvailable ? "is now available" : "is now not available";
                await notificationService.IssueNotifications(NotificationTypes.MenuItemAvailabilityUpdated, $"{menuItem?.Name} {availability} in the menu item", roles);
            }

            var response = new CommonServerResponse
            {
                Status = "Success",
                Message = "Menu item availability updated successfully."
            };

            return new CustomProtocolResponse
            {
                Status = "Success",
                Body = JsonConvert.SerializeObject(response)
            };
        }

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
                    Status = "Failed",
                    Body = JsonConvert.SerializeObject(ex)
                };
            }
        }

        public async static Task<CustomProtocolResponse> GetRecommendation(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<GetRecommendationRequestModel>(body);

                List<GetRecommendationMenuItemResponse> menuItems;

                using (var scope = serviceProvider.CreateScope())
                {
                    var menuItemService = scope.ServiceProvider.GetRequiredService<IMenuItemService>();

                    var recommendedMenuItems = await menuItemService.GetRecommendationByMenuItemCategory(request.MenuItemCategoryId, request.Limit);

                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                    menuItems = mapper.Map<List<MenuItem>, List<GetRecommendationMenuItemResponse>>(recommendedMenuItems);
                }

                return new CustomProtocolResponse
                {
                    Status = "Success",
                    Body = JsonConvert.SerializeObject(menuItems)
                };
            }
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex)
                };
            }
        }

        public async static Task<CustomProtocolResponse> VoteForDailyMenuItem(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<VoteForDailyMenuItemRequest>(body);

                using (var scope = serviceProvider.CreateScope())
                {
                    var voteService = scope.ServiceProvider.GetRequiredService<IDailyRolledOutMenuItemVoteService>();

                    Expression<Func<DailyRolledOutMenuItemVote, bool>> predicate = v => v.UserId == request.UserId && v.DailyRolledOutMenuItemId == request.DailyRolledOutMenuItemId;
                    var duplicateVote = await voteService.GetList<DailyRolledOutMenuItemVote>(predicate: predicate);

                    if (duplicateVote != null && duplicateVote.Count != 0)
                    {
                        throw new AppException("Cannot Vote on already voted item");
                    }

                    var dailyMenuItemVote = new DailyRolledOutMenuItemVote()
                    {
                        DailyRolledOutMenuItemId = request.DailyRolledOutMenuItemId,
                        UserId = request.UserId
                    };
                    await voteService.Add(dailyMenuItemVote);
                }

                var response = new CommonServerResponse
                {
                    Status = "Success",
                    Message = "Voted successfully."
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
                    Body = JsonConvert.SerializeObject(ex)
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
                    Body = JsonConvert.SerializeObject(ex)
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
                    Body = JsonConvert.SerializeObject(ex)
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
                    Body = JsonConvert.SerializeObject(ex)
                };
            }
        }

        public async static Task<CustomProtocolResponse> GiveFeedBackOnMenuItem(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<FeedbackModel>(body);

                using (var scope = serviceProvider.CreateScope())
                {
                    var feedbackService = scope.ServiceProvider.GetRequiredService<IFeedbackService>();

                    await feedbackService.AddFeedBackForMenuItem(request);
                }

                var response = new CommonServerResponse
                {
                    Status = "Success",
                    Message = "Successfully Submitted Feedback."
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
                    Body = JsonConvert.SerializeObject(ex)
                };
            }
        }

        public async static Task<CustomProtocolResponse> GetNotificationsForUser(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var userId = JsonConvert.DeserializeObject<int>(body);
                List<NotificationModel> notifications;
                using (var scope = serviceProvider.CreateScope())
                {
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    notifications = await notificationService.GetNotificationsForUser(userId);
                }

                return new CustomProtocolResponse
                {
                    Status = "Success",
                    Body = JsonHelper.SerializeObjectIgnoringCycles(notifications)
                };
            }
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex)
                };
            }
        }

        public async static Task<CustomProtocolResponse> IssueNotificationForFinalizedMenu(IServiceProvider serviceProvider, string body)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    var tomorrow = DateTime.UtcNow.Date.AddDays(1);
                    string tomorrowDate = tomorrow.ToShortDateString();
                    List<int> roles = new List<int>()
                    {
                        (int)Roles.User
                    };
                    await notificationService.IssueNotifications(NotificationTypes.FinalizeMenu, $"The Menu for {tomorrowDate} has been finalized.", roles);
                }

                var response = new CommonServerResponse
                {
                    Status = "Success",
                    Message = "Successfully Issued Notification for Finalized Menu"
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
                    Body = JsonConvert.SerializeObject(ex)
                };
            }
        }

        public async static Task<CustomProtocolResponse> GenerateDiscardedMenuItems(IServiceProvider serviceProvider, string body)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var discardedMenuItemService = scope.ServiceProvider.GetRequiredService<IDiscardedMenuItemService>();

                    await discardedMenuItemService.GenerateDiscardedMenuItemsForThisMonth();
                }

                var response = new CommonServerResponse
                {
                    Status = "Success",
                    Message = "Successfully Generated Discarded Menu."
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
    }

}
