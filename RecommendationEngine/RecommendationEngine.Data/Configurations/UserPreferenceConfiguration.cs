using Microsoft.EntityFrameworkCore;
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
    public class UserPreferenceConfiguration : BaseConfiguration<UserPreference>
    {
        public override void Configure(EntityTypeBuilder<UserPreference> builder)
        {
            base.Configure(builder);
            builder.HasIndex(up => new { up.UserId, up.CharacteristicId }).IsUnique();
        }
    }
}
