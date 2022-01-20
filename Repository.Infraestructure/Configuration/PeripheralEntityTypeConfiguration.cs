using Gateways.NET.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gateways.NET.Repository.Infraestructure.Configuration
{
    /// <summary>
    /// Configuration of the Peripheral entity
    /// </summary>
    public class PeripheralEntityTypeConfiguration : BaseEntityTypeConfiguration<Peripheral>
    {
        public override void Configure(EntityTypeBuilder<Peripheral> builder)
        {
            base.Configure(builder);            

            // Primary key
            builder.HasKey(k => k.Id);

            // Required properties
            builder.Property(p => p.UID).IsRequired();
            builder.Property(p => p.CreationDate).IsRequired();
            builder.Property(p => p.Status).IsRequired();

            //Indexes
            builder.HasIndex(i => i.UID).IsUnique(true);

            // Relations
            builder.HasOne(n => n.Gateway).WithMany(n => n.Peripherals).HasForeignKey(f => f.GatewayId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
