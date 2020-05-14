using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Infrastructure.Configurations
{
    public class AreaExitConfiguration : IEntityTypeConfiguration<AreaExit>
    {
        public void Configure(EntityTypeBuilder<AreaExit> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
