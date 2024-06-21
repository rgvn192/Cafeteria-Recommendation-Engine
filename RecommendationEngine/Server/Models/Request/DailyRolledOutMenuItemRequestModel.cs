using RecommendationEngine.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.Request
{
    public class DailyRolledOutMenuItemRequestModel
    {
        public int? DailyRolledOutMenuItemId { get; set; }
        public bool? IsShortListed { get; set; }
        public int MenuItemId { get; set; }
        public int MealTypeId { get; set; }
    }
}
