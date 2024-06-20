using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RecommendationEngine.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.DTO
{
    public class MenuItemModel
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Comments { get; set; }
        public bool IsAvailable { get; set; }
        public int MenuItemCategoryId { get; set; }
        public decimal UserLikeability { get; set; }
        public MenuItemCategory MenuItemCategory { get; set; }
    }
}
