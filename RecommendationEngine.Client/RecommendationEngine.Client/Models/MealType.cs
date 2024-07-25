using RecommendationEngine.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class MealType
    {
        public int MealTypeId { get; set; }

        public MealTypes Name { get; set; }
    }
}
