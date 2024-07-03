using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class ViewVotesOnRolledOutMenuItemsResponse
    {
        public int DailyRolledOutMenuItemId { get; set; }
        public bool IsShortListed { get; set; }
        public int MenuItemId { get; set; }
        public int MealTypeId { get; set; }

        public int Votes { get; set; }
    }
}
