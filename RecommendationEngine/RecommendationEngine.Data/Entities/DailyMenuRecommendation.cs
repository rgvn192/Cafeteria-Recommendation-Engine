using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Entities
{
    public class DailyMenuRecommendation : BaseEntity
    {
        [Key]
        public int DailyMenuRecommendationId { get; set; }
        public int Votes { get; set; }
        public bool IsShortListed { get; set; }
        public int MenuItemId { get; set; }
        public int MealTypeId { get; set; }

        public MenuItem MenuItem { get; set; }
        public MealType MealType { get; set; }

    }
}
