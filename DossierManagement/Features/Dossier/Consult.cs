using DossierManagement.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DossierManagement.Features.Dossier
{
    public sealed record Consult
        : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public string? Notes { get; set; } = string.Empty;
    }
}
