using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Core.Models;

namespace WebApi.Infrastructure.Configurations
{
    public class ClientApplicationConfiguration : IEntityTypeConfiguration<ClientApplication>
    {
        public void Configure(EntityTypeBuilder<ClientApplication> builder)
        {
            builder.Property(e => e.ClientName).IsRequired().HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.ClientSecret).IsRequired().HasMaxLength(300).IsUnicode(false);
        }
    }
}
