using RecommendationEngine.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.DTO
{
    public class MenuItemCategoryModel
    {
        public int MenuItemCategoryId { get; set; }
        public string Name { get; set; }
        public List<MenuItem> MenuItems { get; set; }
    }
}
