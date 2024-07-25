using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.Request
{
    public class RemoveUserPreferenceRequest
    {
        public int UserId { get; set; }
        public int UserPreferenceId { get; set; }
    }
}
