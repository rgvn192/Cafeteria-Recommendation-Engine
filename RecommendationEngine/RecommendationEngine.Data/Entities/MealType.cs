using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Entities
{
    public class MealType : BaseEntity
    {
        [Key]
        public int MealTypeId { get; set; }

        [Column(TypeName = ("nvarchar(30)"))]
        public MealTypes Name { get; set; }

        public List<DailyMenuRecommendation> DailyMenuRecommendations { get; set; }
    }

    public enum MealTypes
    {
        Breakfast,
        Lunch,
        Dinner
    }
}
