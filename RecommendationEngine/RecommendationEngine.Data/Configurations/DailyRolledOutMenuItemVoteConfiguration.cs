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
    
    public class DailyRolledOutMenuItemVoteConfiguration : BaseConfiguration<DailyRolledOutMenuItemVote>
    {
        public override void Configure(EntityTypeBuilder<DailyRolledOutMenuItemVote> builder)
        {
            base.Configure(builder);
        }
    }
}
