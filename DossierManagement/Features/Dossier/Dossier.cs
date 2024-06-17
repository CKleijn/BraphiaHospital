using DossierManagement.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DossierManagement.Features.Dossier
{
    public sealed record Dossier
        : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public Patient Patient { get; init; } = new Patient();
        public IList<Consult>? Consults { get; set; }
        public IList<string>? Results { get; set; }
        public IList<string>? Medications { get; set; }
    }
}
