using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class ViewFinalizedRolledOutMenuItemsResponse
    {
        public string Mealtype { get; set; }
        public MenuItemModel MenuItem { get; set; }
    }
}
