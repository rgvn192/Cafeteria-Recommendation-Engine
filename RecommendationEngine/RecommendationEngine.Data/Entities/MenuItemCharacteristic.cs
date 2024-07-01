using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Entities
{
    public class MenuItemCharacteristic : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }

        public int CharacteristicId { get; set; }
        public Characteristic Characteristic { get; set; }
    }
}
