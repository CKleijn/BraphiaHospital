using PatientManagement.Features.Patient.RegisterPatient.Command;
using PatientManagement.Features.Patient.RegisterPatient.Event;

namespace PatientManagement.Features.Patient._Interfaces
{
    public interface IPatientMapper
    {
        Patient RegisterPatientCommandToPatient(RegisterPatientCommand command);
        PatientRegisteredEvent PatientToPatientRegisteredEvent(Patient patient);
    }
}
