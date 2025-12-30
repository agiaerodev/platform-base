using Core.Entities;
using Idata.Data;
using Idata.Data.Entities;
using Idata.Entities.Core;
using Idata.Entities.Test;
using jsreport.Types;
using Microsoft.EntityFrameworkCore;
using Idata.Entities.Iprofile;
using Idata.Data.Entities.Isite;
using Idata.Data.Entities.Iprofile;
using Ihelpers.Helpers;
//appendUsingCommandLine
using Idata.Entities.Icomments;

namespace Platform.Data
{
    public partial class PlatformContext : DbContext
    {
        public PlatformContext() : base()
        {
        }
        public PlatformContext(DbContextOptions<PlatformContext> options) : base(options)
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






        //public virtual DbSet<Log> Logs { get; set; } = null!;
        //public virtual DbSet<Module> Modules { get; set; } = null!;
        //public virtual DbSet<ModuleTranslation> ModuleTranslations { get; set; } = null!;

        ////Iprofile Module
        //public virtual DbSet<Department> Departments { get; set; } = null!;
        //public virtual DbSet<User> Users { get; set; } = null!;
        //public virtual DbSet<Role> Roles { get; set; } = null!;
        //public virtual DbSet<AuthClient> AuthClients { get; set; } = null!;


        ////Tests
        //public virtual DbSet<TestEntity> Tests { get; set; } = null!;

        //appendConsoleLineEntity
        public virtual DbSet<Idata.Entities.Icomments.Icomment> Icomments { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = ConfigurationHelper.GetConfig<string>("ConnectionStrings:DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.CommandTimeout(600);
                    sqlOptions.MigrationsHistoryTable(Ihelpers.Helpers.ConfigurationHelper.GetConfig("App:MigrationHistory"), Ihelpers.Helpers.ConfigurationHelper.GetConfig("App:ShortName"));
                });
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<User>(entity =>
            {
                // Map to the existing table name
                entity.ToTable("users", schema: "iprofile");

                // THIS IS THE KEY: Tell EF to ignore this table during migrations
                entity.ToTable(t => t.ExcludeFromMigrations());
            });

            modelBuilder.Entity<Role>(entity =>
            {
                // Map to the existing table name
                entity.ToTable("roles", schema: "iprofile");

                // THIS IS THE KEY: Tell EF to ignore this table during migrations
                entity.ToTable(t => t.ExcludeFromMigrations());
            });

            modelBuilder.Entity<Department>(entity =>
            {
                // Map to the existing table name
                entity.ToTable("departments", schema: "iprofile");

                // THIS IS THE KEY: Tell EF to ignore this table during migrations
                entity.ToTable(t => t.ExcludeFromMigrations());
            });

            modelBuilder.Entity<AuthClient>(entity =>
            {
                // Map to the existing table name
                entity.ToTable("AuthClients", schema: "iprofile");

                // THIS IS THE KEY: Tell EF to ignore this table during migrations
                entity.ToTable(t => t.ExcludeFromMigrations());
            });

            modelBuilder.Entity<User>()
    .HasMany(u => u.roles)
    .WithMany(r => r.users)
    .UsingEntity<Dictionary<string, object>>(
        "RoleUser",
        j => j
            .HasOne<Role>()
            .WithMany()
            .HasForeignKey("rolesid"),
        j => j
            .HasOne<User>()
            .WithMany()
            .HasForeignKey("usersid"),
        j =>
        {
            j.ToTable("RoleUser", "iprofile", t => t.ExcludeFromMigrations());
        });


            modelBuilder.Entity<User>()
   .HasMany(u => u.departments)
   .WithMany(d => d.users)
   .UsingEntity<Dictionary<string, object>>(
       "DepartmentUser",
       j => j
           .HasOne<Department>()
           .WithMany()
           .HasForeignKey("departmentsid"),
       j => j
           .HasOne<User>()
           .WithMany()
           .HasForeignKey("usersid"),
       j =>
       {
           j.ToTable("DepartmentUser", "iprofile", t => t.ExcludeFromMigrations());
       });

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}
