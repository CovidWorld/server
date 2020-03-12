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
            builder.Property(x => x.DeviceId).IsRequired();
            builder.Property(x => x.Locale).IsRequired();
            builder.Property(x => x.PushToken).IsRequired();
            builder.OwnsOne(x => x.Location, n => { n.WithOwner(); });
            
            var navigation = builder.Metadata.FindNavigation(nameof(Profile.Contacts));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
