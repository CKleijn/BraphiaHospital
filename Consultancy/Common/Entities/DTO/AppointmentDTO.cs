namespace Consultancy.Common.Entities.DTO
{
    public sealed record AppointmentDTO
    {
        public Guid Id { get; set; } = Guid.Empty;
        public PatientDTO Patient { get; set; } = null!;
    }
}
