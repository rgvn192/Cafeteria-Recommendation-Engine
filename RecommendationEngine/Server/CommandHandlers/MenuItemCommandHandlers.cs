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
using System.Text;
using System.Threading.Tasks;

namespace Server.CommandHandlers
{
    public static class MenuItemCommandHandlers
    {
        public async static Task<CustomProtocolResponse> AddMenuItem(IServiceProvider serviceProvider, string body)
        {
            try
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
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex.Message)
                };
            }
        }

        public async static Task<CustomProtocolResponse> UpdateMenuItem(IServiceProvider serviceProvider, string body)
        {
            try
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
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex.Message)
                };
            }
        }

        public async static Task<CustomProtocolResponse> DeleteMenuItem(IServiceProvider serviceProvider, string body)
        {
            try
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
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex.Message)
                };
            }
        }

        public async static Task<CustomProtocolResponse> GetMenuItems(IServiceProvider serviceProvider, string body)
        {
            try
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
            catch (Exception ex)
            {
                return new CustomProtocolResponse
                {
                    Status = "Failure",
                    Body = JsonConvert.SerializeObject(ex.Message)
                };
            }
        }

        public async static Task<CustomProtocolResponse> ToggleMenuItemAvailability(IServiceProvider serviceProvider, string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<int>(body);

                using (var scope = serviceProvider.CreateScope())
                {
                    var menuItemService = scope.ServiceProvider.GetRequiredService<IMenuItemService>();
                    var menuItem = await menuItemService.GetById<MenuItemModel>(request) ?? throw new AppException(ErrorResponse.ErrorEnum.NotFound,
                        LogExtensions.GetLogMessage(nameof(ToggleMenuItemAvailability), null, "No such Menu Item found"));

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
