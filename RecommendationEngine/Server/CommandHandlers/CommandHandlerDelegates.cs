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
                Role = user.Role.Name.ToString(),
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

        public async static Task<CustomProtocolResponse> HandleToggleMenuItemAvailability(IServiceProvider serviceProvider, string body)
        {
            var request = JsonConvert.DeserializeObject<int>(body);

            using (var scope = serviceProvider.CreateScope())
            {
                var menuItemService = scope.ServiceProvider.GetRequiredService<IMenuItemService>();
                var menuItem = await menuItemService.GetById<MenuItemModel>(request);
                menuItem.IsAvailable = !menuItem.IsAvailable;
                await menuItemService.Update(menuItem.MenuItemId, menuItem);
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

    }

}
