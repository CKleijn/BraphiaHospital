using DossierManagement.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DossierManagement.Features.Dossier
{
    public sealed record Result
        : IEntity
    {
        [Key]
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Outcome { get; set; } = string.Empty;
    }
}
