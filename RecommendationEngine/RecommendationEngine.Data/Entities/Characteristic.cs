using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Entities
{
    public class Characteristic : BaseEntity
    {
        [Key]
        public int CharacteristicId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }

        public List<MenuItemCharacteristic> MenuItemCharacteristics { get; set; }
        public List<UserPreference> UserPreferences { get; set; }
    }

}
