using Gateways.NET.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gateways.NET.Repository.Infraestructure.Configuration
{
    /// <summary>
    /// Configuration of the Gateway entity
    /// </summary>
    public class GatewayEntityTypeConfiguration : BaseEntityTypeConfiguration<Gateway>
    {
        public override void Configure(EntityTypeBuilder<Gateway> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(k => k.Id);

            // Required properties
            builder.Property(p => p.SerialNumber).IsRequired();            
            builder.Property(p => p.IpAddress).IsRequired();

            //Indexes
            builder.HasIndex(i => i.SerialNumber).IsUnique(true);

            // Relations
            builder.HasMany(n => n.Peripherals).WithOne(n => n.Gateway).HasForeignKey(f => f.GatewayId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
