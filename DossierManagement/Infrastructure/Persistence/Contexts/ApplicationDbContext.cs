using DossierManagement.Features.Dossier;
using Microsoft.EntityFrameworkCore;

namespace DossierManagement.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : DbContext(options)
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Dossier> Dossiers { get; set; }
        public DbSet<Consult> Consults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Dossier>()
                .HasOne(d => d.Patient)
                .WithOne()
                .HasForeignKey<Dossier>(d => d.PatientId);

            modelBuilder.Entity<Dossier>()
                .HasMany(d => d.Consults)
                .WithOne();
        }
    }
}
