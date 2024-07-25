using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class GetRecommendationMenuItemResponse
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Comments { get; set; }
        public bool IsAvailable { get; set; }
        public int MenuItemCategoryId { get; set; }

        public decimal UserLikeability { get; set; }

        public decimal AverageRating { get; set; }
    }
}
