using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Infrastructure.Configurations
{
    public class PushNonceConfiguration : IEntityTypeConfiguration<PushNonce>
    {
        public void Configure(EntityTypeBuilder<PushNonce> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .IsRequired();
            builder.HasIndex(x => x.Id)
                .IsUnique();
        }
    }
}
