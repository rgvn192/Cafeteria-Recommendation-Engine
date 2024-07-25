using RecommendationEngine.Client.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class DiscardedMenuItemFeedback
    {
        public int DiscardedMenuItemFeedbackId { get; set; }

        public int DiscardedMenuItemId { get; set; }
        public int UserId { get; set; }

        public string Feedback { get; set; }

    }
}
