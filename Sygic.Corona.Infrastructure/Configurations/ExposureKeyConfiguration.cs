using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Infrastructure.Configurations
{
    public class ExposureKeyConfiguration : IEntityTypeConfiguration<ExposureKey>
    {
        public void Configure(EntityTypeBuilder<ExposureKey> builder)
        {
            builder.Property(x => x.Id)
                .HasDefaultValueSql("NEWID()");
        }
    }
}
