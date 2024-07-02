using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Entities
{
    public class DiscardedMenuItemFeedback : BaseEntity
    {
        public int DiscardedMenuItemFeedbackId { get; set; }

        public int DiscardedMenuItemId { get; set; }
        public int UserId { get; set; }

        public string Feedback { get; set; }

        public DiscardedMenuItem DiscardedMenuItem { get; set; }
        public User User { get; set; }
    }
}
