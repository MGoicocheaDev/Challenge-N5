
using Microsoft.EntityFrameworkCore;
using web_api_lib_data.Models;

namespace web_api_lib_data.Context
{
    public partial class WebApiContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public WebApiContext()
        {
        }

        public WebApiContext(DbContextOptions<WebApiContext> options)
            : base(options)
        {
        }

        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<PermissionType> PermissionTypes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            });

            modelBuilder.Entity<PermissionType>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Id)
                .ValueGeneratedOnAdd();
            });

            /// Seed Data
            modelBuilder.Entity<PermissionType>().HasData(
                new PermissionType
                {
                    Id = 1,
                    Descripcion = "Read"
                },
                new PermissionType
                {
                    Id = 2,
                    Descripcion = "Read/Write"
                });
        }

    }
}
