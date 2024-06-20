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

    public class DailyMenuRecommendationConfiguration : BaseConfiguration<DailyMenuRecommendation>
    {
        public override void Configure(EntityTypeBuilder<DailyMenuRecommendation> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.IsShortListed).HasDefaultValue(false);
        }
    }
}
