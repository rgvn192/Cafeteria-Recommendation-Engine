using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class RolledOutMenuItem
    {
        public int DailyRolledOutMenuItemId { get; set; }

        public GetRecommendationMenuItemResponse MenuItem { get; set; }

        public MealType MealType { get; set; }
    }
}
