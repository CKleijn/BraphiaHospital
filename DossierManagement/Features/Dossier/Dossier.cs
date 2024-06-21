using DossierManagement.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DossierManagement.Features.Dossier
{
    public sealed record Dossier
        : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
        public IList<Consult>? Consults { get; set; }
        public IList<string>? Medications { get; set; }
    }
}
