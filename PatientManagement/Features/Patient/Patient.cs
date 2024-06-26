using PatientManagement.Common.Aggregates;
using PatientManagement.Events;

namespace PatientManagement.Features.Patient
{
    public sealed class Patient
        : AggregateRoot
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.Today;
        public string BSN { get; set; } = string.Empty;
        public string Address {  get; set; } = string.Empty;

        public void Apply(PatientRegisteredEvent @event)
        {
            Id = @event.Patient.Id;
            FirstName = @event.Patient.FirstName;
            LastName = @event.Patient.LastName;
            DateOfBirth = @event.Patient.DateOfBirth;
            BSN = @event.Patient.BSN;
            Address = @event.Patient.Address;
        }
    }
}
