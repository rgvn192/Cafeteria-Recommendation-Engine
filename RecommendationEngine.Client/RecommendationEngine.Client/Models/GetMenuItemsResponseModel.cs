using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class GetMenuItemsResponseModel
    {
        public List<MenuItemModel> MenuItems { get; set; } = new List<MenuItemModel>();
    }
}
