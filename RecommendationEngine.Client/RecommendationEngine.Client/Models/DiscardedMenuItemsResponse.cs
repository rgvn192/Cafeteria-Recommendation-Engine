using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class DiscardedMenuItemsResponse
    {
        public int DiscardedMenuItemId { get; set; }

        public int MenuItemId { get; set; }

        public GetRecommendationMenuItemResponse MenuItem { get; set; }
    }
}
