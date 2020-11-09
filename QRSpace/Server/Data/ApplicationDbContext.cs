using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QRSpace.Server.Entities;

namespace QRSpace.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, ulong>
    {
        public DbSet<ShogiRecord> ShogiRecords { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : base(opts)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ShogiRecord>().HasOne<ApplicationUser>().WithMany();

            builder.Entity<ApplicationUser>(entityTypeBuilder =>
            {
                entityTypeBuilder.Property(u => u.Id).ValueGeneratedNever();
            });

            builder.Entity<ApplicationRole>(entityTypeBuilder =>
            {
                entityTypeBuilder.Property(r => r.Id).ValueGeneratedNever();
            });

            base.OnModelCreating(builder);
        }
    }
}