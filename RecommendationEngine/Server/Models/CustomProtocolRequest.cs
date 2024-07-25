using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    internal class CustomProtocolRequest
    {
        public string Command { get; set; }
        public string Role { get; set; }
        public string Body { get; set; }
    }
}
