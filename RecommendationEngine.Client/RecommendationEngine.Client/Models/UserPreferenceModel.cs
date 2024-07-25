using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Client.Models
{
    public class UserPreferenceModel
    {
        public int UserPreferenceId { get; set; }
        public int UserId { get; set; }

        public int CharacteristicId { get; set; }
        public Characteristic Characteristic { get; set; }
    }
}
