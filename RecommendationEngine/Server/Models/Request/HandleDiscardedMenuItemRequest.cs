using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.Request
{
    public class HandleDiscardedMenuItemRequest
    {
        public int DiscardedMenuItemId { get; set; }
        public bool MakeAvailable { get; set; }
    }
}
