using RecommendationEngine.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.DTO
{
    public class UserPreferenceModel
    {
        public int UserPreferenceId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int CharacteristicId { get; set; }
        public Characteristic Characteristic { get; set; }
    }
}
