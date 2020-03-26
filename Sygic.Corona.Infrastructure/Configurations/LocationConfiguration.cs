using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Infrastructure.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasPartitionKey(x => x.ProfileId);
            builder.Property(x => x.ProfileId)
                .HasConversion(x => x.ToString(), x => uint.Parse(x));
        }
    }
}
