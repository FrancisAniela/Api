using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Core.Models;

namespace WebApi.Infrastructure.Configurations
{
    public class ArticuloConfiguration : IEntityTypeConfiguration<Articulo>
    {
        public void Configure(EntityTypeBuilder<Articulo> builder)
        {
            builder.Property(e => e.Descripcion).IsRequired().HasMaxLength(200).IsUnicode(false).IsRequired();

            builder.Property(e => e.Nombre).IsRequired().HasMaxLength(100).IsUnicode(false).IsRequired();

            builder.Property(e => e.FechaCreacion).HasColumnType("datetime").IsRequired();

            builder.Property(e => e.FechaModificacion).HasColumnType("datetime");

            builder.Property(e => e.Codigo).IsRequired().HasMaxLength(100).IsUnicode(false).IsRequired();


            builder.Property(e => e.Precio1)
                .IsRequired()
                .HasColumnType("decimal(18,4)");


            builder.Property(e => e.Precio2)
                .IsRequired()
                .HasColumnType("decimal(18,4)");

            builder.Property(e => e.Imagen).IsRequired().IsUnicode(false);


            builder.HasOne(d => d.Estado)
               .WithMany(p => p.Articulos)
               .HasForeignKey(d => d.IdEstado)
               .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Articulo_EstadoArticulo");
        }
    }
}
