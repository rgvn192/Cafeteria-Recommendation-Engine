using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecommendationEngine.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationEngine.Data.Configuration
{
    public class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        protected static string Pluralize()
        {
            return typeof(TEntity).Name + "s";
        }

        protected bool SetTableName { get; set; } = true;

        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            if (SetTableName)
            {
                builder.ToTable(Pluralize());
            }

            builder.Property(x => x.CreatedDatetime).HasDefaultValueSql("getutcdate()");
            builder.Property(x => x.ModifiedDatetime).HasDefaultValueSql("getutcdate()");
        }
    }
}
