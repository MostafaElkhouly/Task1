using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Configration.EntitiesProperties.Properties.Base
{
    public class PropertiesBase<TEntity> : IEntityTypeConfiguration<TEntity> 
        where TEntity : EntityBase
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            
        builder.Property(b => b.DateOfCreate)
        .HasDefaultValueSql("GETUTCDATE()");

        }
    }

}
