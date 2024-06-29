using RecommendationEngine.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.Response
{
    public class ViewVotesOnRolledOutMenuItemsResponse
    {
        public int DailyRolledOutMenuItemId { get; set; }
        public bool IsShortListed { get; set; }
        public int MenuItemId { get; set; }
        public int MealTypeId { get; set; }

        public MenuItem MenuItem { get; set; }
        public MealType MealType { get; set; }

        public int Votes { get; set; }
    }
}
