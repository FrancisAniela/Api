using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Core.Models;

namespace WebApi.Infrastructure.Configurations
{
    public class SeguridadConfiguration : IEntityTypeConfiguration<Seguridad>
    {
        public void Configure(EntityTypeBuilder<Seguridad> builder)
        {
            builder.Property(e => e.Token)
                .HasMaxLength(100)
                 .IsRequired()
                .IsFixedLength();
        }
    }

}
