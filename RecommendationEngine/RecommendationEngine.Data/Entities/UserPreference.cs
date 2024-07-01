using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Entities
{
    public class UserPreference : BaseEntity
    {
        [Key]
        public int UserPreferenceId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int CharacteristicId { get; set; }
        public Characteristic Characteristic { get; set; }
    }
}
