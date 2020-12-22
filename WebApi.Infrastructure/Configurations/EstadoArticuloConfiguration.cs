using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Core.Models;

namespace WebApi.Infrastructure.Configurations
{
    public class EstadoArticuloConfiguration : IEntityTypeConfiguration<EstadoArticulo>
    {
        public void Configure(EntityTypeBuilder<EstadoArticulo> builder)
        {
            builder.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);
        }
    }
}
