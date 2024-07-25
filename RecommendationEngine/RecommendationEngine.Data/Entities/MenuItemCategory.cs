using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Entities
{
    public class MenuItemCategory : BaseEntity
    {
        [Key]
        public int MenuItemCategoryId { get; set; }
        public string Name { get; set; }

        public List<MenuItem> MenuItems { get; set; }
    }
}
