using System.ComponentModel.DataAnnotations;

namespace DossierManagement.Features.Dossier
{
    public sealed class Consult
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid PatientId { get; set; } = Guid.Empty;
        public string? Notes { get; set; } = string.Empty;
    }
}
