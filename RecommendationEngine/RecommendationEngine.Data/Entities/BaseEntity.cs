using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedDatetime { get; set; }
        public DateTime? ModifiedDatetime { get; set; }
    }
}
