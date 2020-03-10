using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sygic.Corona.Domain;

namespace Sygic.Corona.Infrastructure.Configurations
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.Location, n => { n.WithOwner(); });
        }
    }
}
