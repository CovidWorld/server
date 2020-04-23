using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Infrastructure.Configurations
{
    public class AlertConfiguration : IEntityTypeConfiguration<Alert>
    {
        public void Configure(EntityTypeBuilder<Alert> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasPartitionKey(x => x.ProfileId);
            builder.Property(x => x.ProfileId)
                .HasConversion(x => x.ToString(), x => uint.Parse(x));
        }
    }
}
