using Microsoft.EntityFrameworkCore;
using PatientManagement.Features.Patient;

namespace PatientManagement.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : DbContext(options)
    {
        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}
