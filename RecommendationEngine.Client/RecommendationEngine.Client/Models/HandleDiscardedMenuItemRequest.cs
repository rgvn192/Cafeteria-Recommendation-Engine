using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class HandleDiscardedMenuItemRequest
    {
        public int DiscardedMenuItemId { get; set; }
        public bool MakeAvailable { get; set; }
    }
}
