using Microsoft.EntityFrameworkCore;
using Consultancy.Features.Consult;

namespace Consultancy.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : DbContext(options)
    {
        public DbSet<Consult> Consults { get; set; }
        public DbSet<Consult> Surveys { get; set; }
        public DbSet<Consult> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Consult>()
                .HasOne(c => c.Survey)
                .WithMany(s => s.Consults)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Survey)
                .WithMany(s => s.Questions)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
