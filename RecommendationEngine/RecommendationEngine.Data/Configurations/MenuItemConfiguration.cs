using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecommendationEngine.Data.Configuration;
using RecommendationEngine.Data.Entities;


namespace RecommendationEngine.Data.Configurations
{
    public class MenuItemConfiguration : BaseConfiguration<MenuItem>
    {
        public override void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.IsAvailable).HasDefaultValue(true);
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
