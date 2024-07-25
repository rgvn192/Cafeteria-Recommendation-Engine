using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class GetMenuItemsRequestModel
    {
        public int Limit { get; set; }
        public int Offset { get; set; }

    }
}
