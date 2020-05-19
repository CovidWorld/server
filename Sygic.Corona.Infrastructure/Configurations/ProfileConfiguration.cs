using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Sygic.Corona.Domain;
using System;

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

            builder.OwnsOne(x => x.ClientInfo, n => 
            { 
                n.WithOwner(); 
                n.Property(p => p.OperationSystem)
                    .HasConversion(x => x.ToString(),
                        x => (Platform)Enum.Parse(typeof(Platform), x));
            });
            builder.OwnsOne(x => x.QuarantineAddress, n => 
            { 
                n.WithOwner(); 
                n.Property(p => p.Latitude)
                    .HasColumnType("decimal(11, 8)");
                n.Property(p => p.Longitude)
                    .HasColumnType("decimal(11, 8)");
            });

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

            builder.HasMany(b => b.AreaExits)
                .WithOne()
                .HasForeignKey(x => x.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.PresenceChecks)
                .WithOne()
                .HasForeignKey(x => x.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
