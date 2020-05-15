using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sygic.Corona.Domain;
using System;

namespace Sygic.Corona.Infrastructure.Configurations
{
    public class PresenceCheckConfiguration : IEntityTypeConfiguration<PresenceCheck>
    {
        public void Configure(EntityTypeBuilder<PresenceCheck> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id)
                .IsUnique();
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Status)
                .HasConversion(x => x.ToString(),
                        x => (PresenceCheckStatus)Enum.Parse(typeof(PresenceCheckStatus), x));
        }
    }
}
