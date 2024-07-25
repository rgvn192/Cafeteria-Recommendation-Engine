using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.DTO
{
    public class DiscardedMenuItemFeedbackModel
    {
        public int DiscardedMenuItemFeedbackId { get; set; }

        public int DiscardedMenuItemId { get; set; }
        public int UserId { get; set; }

        public string Feedback { get; set; }

    }
}
