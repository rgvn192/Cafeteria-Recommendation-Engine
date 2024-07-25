using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Entities
{
    public class DiscardedMenuItem : BaseEntity
    {
        [Key]
        public int DiscardedMenuItemId { get; set; }

        public int MenuItemId { get; set; }

        public MenuItem MenuItem { get; set; }

        public List<DiscardedMenuItemFeedback> DiscardedMenuItemFeedbacks { get; set; }

    }
}
