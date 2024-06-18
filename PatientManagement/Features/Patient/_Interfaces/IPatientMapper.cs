using PatientManagement.Events;
using PatientManagement.Features.Patient.RegisterPatient;

namespace PatientManagement.Features.Patient._Interfaces
{
    public interface IPatientMapper
    {
        Patient RegisterPatientCommandToPatient(RegisterPatientCommand command);
        PatientRegisteredEvent PatientToPatientRegisteredEvent(Patient patient);
    }
}
