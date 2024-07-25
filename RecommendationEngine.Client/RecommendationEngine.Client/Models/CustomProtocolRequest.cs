using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class CustomProtocolRequest
    {
        public string Command { get; set; }
        public string Role { get; set; } // Ensure the Role property is included
        public string Body { get; set; }
    }
}
