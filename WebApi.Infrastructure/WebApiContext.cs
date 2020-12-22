using Microsoft.EntityFrameworkCore;
using WebApi.Core.Models;
using WebApi.Infrastructure.Configurations;

namespace WebApi.Infrastructure
{
    public partial class WebApiContext : DbContext
    {
        public WebApiContext()
        {
        }

        public WebApiContext(DbContextOptions<WebApiContext> options)
    : base(options)
        {
        }


        public virtual DbSet<Articulo> Articulo { get; set; }
        public virtual DbSet<EstadoArticulo> EstadoArticulo { get; set; }

        //public virtual DbSet<AppConfig> AppConfig { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=db1d4fd878a909492098d8abe900efdaf9;User ID=sa;Password=wms123456;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new EstadoArticuloConfiguration());

            modelBuilder.ApplyConfiguration(new ArticuloConfiguration());
            
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
