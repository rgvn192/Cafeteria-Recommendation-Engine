using AutoMapper;
using Microsoft.Extensions.Logging;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Interface;
using Server.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    
    public class MenuItemService : CrudBaseService<MenuItem>, IMenuItemService
    {
        public MenuItemService(IMenuItemRepository menuItemRepository, IMapper mapper, ILogger<MenuItemService> logger) :
            base(menuItemRepository, mapper, logger)
        {
        }

        protected override List<string> ModifiableProperties => new()
        {
            nameof(MenuItem.Name),
            nameof(MenuItem.IsAvailable),
            nameof(MenuItem.Price),
            nameof(MenuItem.UserLikeability),
            nameof(MenuItem.Comments),
        };
    }
}
