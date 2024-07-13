using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RecommendationEngine.Data.Entities;
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
    public static class RecommendationCommandHandlers
    {
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
                    Body = JsonConvert.SerializeObject(ex.Message)
                };
            }
        }
    }
}
