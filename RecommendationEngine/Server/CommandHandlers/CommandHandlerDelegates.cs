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
            var role = await authService.AuthenticateUser(loginRequest);

            var loginResponse = new LoginResponseModel
            {
                Role = role
            };

            return new CustomProtocolResponse
            {
                Status = role != null ? "Success" : "Failed",
                Body = JsonConvert.SerializeObject(loginResponse)
            };
        }

        public static Task<CustomProtocolResponse> HandleAddMenuItem(IServiceProvider serviceProvider, string body)
        {
            //var request = JsonConvert.DeserializeObject<AddMenuItemRequestModel>(body);

            //using (var scope = serviceProvider.CreateScope())
            //{
            //    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //    var menuItem = new MenuItem
            //    {
            //        Name = request.Name,
            //        Type = request.Type,
            //        Price = request.Price,
            //        IsAvailable = request.IsAvailable
            //    };

            //    context.MenuItems.Add(menuItem);
            //    await context.SaveChangesAsync();
            //}

            //var response = new AddMenuItemResponseModel
            //{
            //    Status = "Success",
            //    Message = "Menu item added successfully."
            //};

            return Task.FromResult(new CustomProtocolResponse
            {
                Status = "Success",
                //Body = JsonConvert.SerializeObject(response)
            });
        }
    }

}
