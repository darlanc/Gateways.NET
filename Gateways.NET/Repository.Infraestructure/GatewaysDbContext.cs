using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Gateways.NET.Repository.Infraestructure
{
    /// <summary>
    /// Gateways.NET Database context
    /// </summary>
    public class GatewaysDbContext : DbContext
    {   
        public GatewaysDbContext(DbContextOptions<GatewaysDbContext> options)
            : base(options) { }

        /// <summary>
        /// Register DataBase Configuration Dyamically
        /// </summary>
        /// <param name="modelBuilder">Model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var typesToRegister =
                from type in Assembly.GetExecutingAssembly().GetTypes()
                where !string.IsNullOrEmpty(type.Namespace)
                            && type.BaseType != null
                            && type.BaseType.IsGenericType
                            && type.BaseType.GetGenericTypeDefinition() == typeof(BaseEntityTypeConfiguration<>)
                select type;

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
