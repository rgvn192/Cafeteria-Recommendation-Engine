using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Entities
{
    public class MenuItem : BaseEntity
    {
        [Key]
        public int MenuItemId { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string Name { get; set; }
        public decimal Price { get; set; }

        [Column(TypeName = "varchar(70)")]
        public string? Comments { get; set; }
        public bool IsAvailable { get; set; }
        public int MenuItemCategoryId { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal UserLikeability { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal AverageRating { get; set; }

        public MenuItemCategory MenuItemCategory { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        public List<DailyRolledOutMenuItem> DailyRolledOutMenuItems { get; set; }
    }
}
