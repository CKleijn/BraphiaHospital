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

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}
