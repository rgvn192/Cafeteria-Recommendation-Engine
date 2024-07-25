using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.Request
{
    public class VoteForDailyMenuItemRequest
    {
        public int DailyRolledOutMenuItemId { get; set; }
        public int UserId { get; set; }
    }
}
