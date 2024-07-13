using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class RemoveUserPreferenceRequest
    {
        public int UserId { get; set; }
        public int UserPreferenceId { get; set; }
    }
}
