namespace Consultancy.Common.Entities.DTO
{
    public sealed record PatientDTO
    {
        public Guid Id { get; set; } = Guid.Empty;
    }
}
