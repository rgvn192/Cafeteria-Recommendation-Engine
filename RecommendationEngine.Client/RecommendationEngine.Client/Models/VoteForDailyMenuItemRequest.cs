using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class VoteForDailyMenuItemRequest
    {
        public int DailyRolledOutMenuItemId { get; set; }
        public int UserId { get; set; }
    }
}
