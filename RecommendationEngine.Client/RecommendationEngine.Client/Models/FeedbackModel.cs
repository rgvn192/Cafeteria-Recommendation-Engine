using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class FeedbackModel
    {
        public int FeedbackId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public int MenuItemId { get; set; }

    }
}
