using Idata.Data.Entities;
using Idata.Entities.Core;
using Ihelpers.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Idata.Entities.Iprofile;
using Idata.Data.Entities.Isite;
using Idata.Data.Entities.Iprofile;
using Idata.Entities.Test;
using System.Reflection;
//appendUsingCommandLine
using Idata.Entities.Isite;

namespace Idata.Data
{
    public partial class IdataContext : DbContext
    {

        private static string? _ConectionString = null;

        public IdataContext() : base()
        {
        }      
        public IdataContext(DbContextOptions<IdataContext> options) : base(options)
        {
        }

        public sealed override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is EntityBase && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                switch (entityEntry.State)
                {
                    case EntityState.Deleted:
                        ((EntityBase)entityEntry.Entity).deleted_at = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        ((EntityBase)entityEntry.Entity).updated_at = DateTime.UtcNow;
                        entityEntry.Property("created_at").IsModified = false;
                        entityEntry.Property("created_by").IsModified = false;
                        break;
                    case EntityState.Added:
                        ((EntityBase)entityEntry.Entity).created_at = DateTime.UtcNow;

                        break;
                }

            }

            return base.SaveChangesAsync();
        }


        public virtual DbSet<Log> Logs { get; set; } = null!;

        public virtual DbSet<Entities.Isite.Module> Modules { get; set; } = null!;
        public virtual DbSet<ModuleTranslation> ModuleTranslations { get; set; } = null!;
        public virtual DbSet<Revision> Revisions { get; set; } = null!;

        //Iprofile
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<AuthClient> AuthClients { get; set; } = null!;

        //appendConsoleLineEntity

        #region Test Entities
        public virtual DbSet<TestEntity> Tests { get; set; } = null!;
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                if (!Ihelpers.Helpers.ConfigurationHelper.isAzureFunction)
                {

                    IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                   .AddJsonFile("appsettings.json")
                   .Build();
                    var connectionString = configuration.GetConnectionString("DefaultConnection");
                    optionsBuilder.UseSqlServer(connectionString, options => options.CommandTimeout(600));
                }
                else
                {
                    var connectionString = ConfigurationHelper.GetConfig<string>("ConnectionStrings:DefaultConnection");
                    optionsBuilder.UseSqlServer(connectionString, options => options.CommandTimeout(600));
                }
            }

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<Department>(entity =>
            {

                entity.Property(e => e.options).HasColumnType("text");

                entity.Property(e => e.title)
                    .HasMaxLength(191)
                    .IsUnicode(false);
            });



            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.email).IsUnique();
            });


            modelBuilder.Entity<Idata.Data.Entities.Isite.Module>()
                .Property(m => m.priority)
                .HasDefaultValue(1);

            modelBuilder.Entity<Department>(entity =>
            {
                entity.Property(e => e.options).HasColumnType("text");
                entity.Property(e => e.title)
                    .HasMaxLength(191)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Idata.Data.Entities.Isite.Module>()
                .Property(m => m.priority)
                .HasDefaultValue(1);
        }
 

        /// <summary>
        /// Configures the connection string for this context.
        /// </summary>
        /// <param name="ConectionString">The connection string to use.</param>
        public static void ConfigureContext(string ConectionString)
        {
            // Store the connection string for later use.
            _ConectionString = ConectionString;
        }

       
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}
