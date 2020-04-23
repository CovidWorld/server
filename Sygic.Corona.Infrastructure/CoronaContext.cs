using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sygic.Corona.Domain;
using Sygic.Corona.Domain.Common;

namespace Sygic.Corona.Infrastructure
{
    public class CoronaContext : DbContext, IUnitOfWork
    {
        protected CoronaContext()
        {
            
        }
        public CoronaContext(DbContextOptions options) : base(options)
        {
            
        }

        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Alert> Alerts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoronaContext).GetTypeInfo().Assembly);
        }
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
