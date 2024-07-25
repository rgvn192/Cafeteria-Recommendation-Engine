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
    public class RecommendationCommandHandlers
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IMapper _mapper;

        public RecommendationCommandHandlers(IMenuItemService menuItemService, IMapper mapper)
        {
            _menuItemService = menuItemService;
            _mapper = mapper;
        }
        public async Task<CustomProtocolResponse> GetRecommendation(string body)
        {
            try
            {
                var request = JsonConvert.DeserializeObject<GetRecommendationRequestModel>(body);

                List<GetRecommendationMenuItemResponse> menuItems;

                var recommendedMenuItems = await _menuItemService.GetRecommendationByMenuItemCategory(request.MenuItemCategoryId, request.Limit);

                menuItems = _mapper.Map<List<MenuItem>, List<GetRecommendationMenuItemResponse>>(recommendedMenuItems);

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
