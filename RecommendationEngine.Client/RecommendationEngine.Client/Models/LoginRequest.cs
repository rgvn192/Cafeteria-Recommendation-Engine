using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class LoginRequest
    {
        public string endpoint { get; set; }
        public string username { get; set; }
        public string password { get; set; }

    }
}
