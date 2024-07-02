using RecommendationEngine.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.Response
{
    public class RolledOutMenuItem
    {
        public int DailyRolledOutMenuItemId { get; set; }

        public MenuItem MenuItem { get; set; }

        public MealType MealType { get; set; }
    }
}
