using Domain.Configration.EntitiesProperties.Properties.Base;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Configration.EntitiesProperties.Properties
{
    public class PeripheralDeviceProperties : PropertiesBase<PeripheralDevice>
    {
        public override void Configure(EntityTypeBuilder<PeripheralDevice> builder)
        {
            builder.HasIndex(e => e.UID).IsUnique();
            


            base.Configure(builder);
        }
    }
}
