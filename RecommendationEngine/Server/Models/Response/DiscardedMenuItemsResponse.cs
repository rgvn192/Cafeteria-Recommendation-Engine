using RecommendationEngine.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.Response
{
    public class DiscardedMenuItemsResponse
    {
        public int DiscardedMenuItemId { get; set; }

        public int MenuItemId { get; set; }

        public MenuItem MenuItem { get; set; }
    }
}
