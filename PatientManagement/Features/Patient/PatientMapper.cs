using PatientManagement.Features.Patient._Interfaces;
using PatientManagement.Features.Patient.RegisterPatient.Command;
using PatientManagement.Features.Patient.RegisterPatient.Event;
using Riok.Mapperly.Abstractions;

namespace PatientManagement.Features.Patient
{
    [Mapper]
    public partial class PatientMapper
        : IPatientMapper
    {
        public partial Patient RegisterPatientCommandToPatient(RegisterPatientCommand command);
        public partial PatientRegisteredEvent PatientToPatientRegisteredEvent(Patient patient);
    }
}
