using MediatR;
using PatientManagement.Common.Abstractions;
using PatientManagement.Features.Patient;

namespace PatientManagement.Events
{
    public sealed class PatientRegisteredEvent(Patient patient)
        : Event, INotification
    {
        public Patient Patient { get; set; } = patient;
    }
    //    Guid id,
    //    string firstName,
    //    string lastName,
    //    DateTime dateOfBirth,
    //    string bsn,
    //    string address)
    //    : Event, INotification
    //{
    //    public Guid Id { get; private set; } = id;
    //    public string FirstName { get; private set; } = firstName;
    //    public string LastName { get; private set; } = lastName;
    //    public DateTime DateOfBirth { get; private set; } = dateOfBirth;
    //    public string BSN { get; private set; } = bsn;
    //    public string Address { get; private set; } = address;

    //    //public PatientRegisteredEvent(string type, string payload, int version)
    //    //    : base(type, payload, version)
    //    //{
    //    //}
    //}
}
