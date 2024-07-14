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
    public class MenuItemCommandHandlers
    {
        private readonly IMenuItemService _menuItemService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public MenuItemCommandHandlers(IMenuItemService menuItemService, INotificationService notificationService, IMapper mapper)
        {
            _menuItemService = menuItemService;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<CustomProtocolResponse> AddMenuItem(string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<MenuItemModel>(body);

                await _menuItemService.Add(request);

                List<int> roles = new()
                    {
                        (int)Roles.User,
                        (int)Roles.Chef
                    };
                await _notificationService.IssueNotifications(NotificationTypes.NewMenuItemAdded, $"{request?.Name} has been added to Menu", roles);

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

        public async Task<CustomProtocolResponse> UpdateMenuItem(string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<UpdateMenuItemRequestModel>(body);

                var menuItem = await _menuItemService.GetById<MenuItemModel>(request.MenuItemId);

                menuItem = _mapper.Map(request, menuItem);

                await _menuItemService.Update(request.MenuItemId, menuItem);

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

        public async Task<CustomProtocolResponse> DeleteMenuItem(string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<int>(body);

                await _menuItemService.Delete(request);

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

        public async Task<CustomProtocolResponse> GetMenuItems(string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<GetMenuItemsRequestModel>(body);

                List<MenuItemModel> menuItems;

                menuItems = await _menuItemService.GetList<MenuItemModel>(null, null, null, request.Limit, request.Offset);

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

        public async Task<CustomProtocolResponse> ToggleMenuItemAvailability(string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<int>(body);

                var menuItem = await _menuItemService.GetById<MenuItemModel>(request) ?? throw new AppException(ErrorResponse.ErrorEnum.NotFound,
                    LogExtensions.GetLogMessage(nameof(ToggleMenuItemAvailability), null, "No such Menu Item found"));

                menuItem.IsAvailable = !menuItem.IsAvailable;
                await _menuItemService.Update(menuItem.MenuItemId, menuItem);

                List<int> roles = new()
                    {
                        (int)Roles.User,
                        (int)Roles.Chef
                    };
                string availability = menuItem.IsAvailable ? "is now available" : "is now not available";
                await _notificationService.IssueNotifications(NotificationTypes.MenuItemAvailabilityUpdated, $"{menuItem?.Name} {availability} in the menu item", roles);

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
