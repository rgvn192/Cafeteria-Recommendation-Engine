using RecommendationEngine.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.Request
{
    public class AddMenuItemRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int MenuItemCategoryId { get; set; }
    }
}
