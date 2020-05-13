using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
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
            builder.Property(x => x.PushToken).IsRequired(false);

            builder.OwnsOne(x => x.AreaExit, n => { n.WithOwner(); });
            builder.OwnsOne(x => x.ClientInfo, n => { n.WithOwner(); });

            //var navigation = builder.Metadata.FindNavigation(nameof(Profile.Contacts));
            //navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(b => b.Contacts)
                .WithOne()
                .HasForeignKey(x => x.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.Locations)
                .WithOne()
                .HasForeignKey(x => x.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.Alerts)
                .WithOne()
                .HasForeignKey(x => x.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
