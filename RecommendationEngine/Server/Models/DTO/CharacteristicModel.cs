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
    public class CharacteristicModel
    {
        public int CharacteristicId { get; set; }

        public string Name { get; set; }

        public List<MenuItemCharacteristic> MenuItemCharacteristics { get; set; }
        public List<UserPreference> UserPreferences { get; set; }
    }
}
