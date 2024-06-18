using DossierManagement.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DossierManagement.Features.Dossier
{
    public sealed record Medication
        : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Medicine { get; set; } = string.Empty;
    }
}
