using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecommendationEngine.Data.Configuration;
using RecommendationEngine.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Configurations
{    
    public class MenuItemCharacteristicConfiguration : BaseConfiguration<MenuItemCharacteristic>
    {
        public override void Configure(EntityTypeBuilder<MenuItemCharacteristic> builder)
        {
            base.Configure(builder);
        }
    }
}
