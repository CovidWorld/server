using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
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
        public virtual DbSet<ExposureKey> ExposureKeys { get; set; }
        public virtual DbSet<PushNonce> PushNonces { get; set; }
        public virtual DbSet<AreaExit> AreaExits { get; set; }

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

    public class CoronaContextDesignFactory : IDesignTimeDbContextFactory<CoronaContext>
    {
        public CoronaContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Directory.GetCurrentDirectory() + "/../Sygic.Corona.Profile/local.settings.json")
                .Build();
            var configurationSection = configuration.GetSection("Values");

            var optionsBuilder = new DbContextOptionsBuilder<CoronaContext>();
            var connectionString = configurationSection["SqlDbConnection"];
            optionsBuilder.UseSqlServer(connectionString);
            return new CoronaContext(optionsBuilder.Options);
        }
    }
}
