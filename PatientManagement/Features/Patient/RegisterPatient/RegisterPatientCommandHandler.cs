using MediatR;
using PatientManagement.Common.Annotations;
using PatientManagement.Infrastructure.Persistence;
using System.Text.Json;

namespace PatientManagement.Features.Patient.RegisterPatient
{
    public sealed class RegisterPatientCommandHandler(IEventStore eventStore)
        : IRequestHandler<RegisterPatientCommand>
    {
        public async Task Handle(
            RegisterPatientCommand request,
            CancellationToken cancellationToken)
        {
            var result = await eventStore
                .AddEvent(
                EventKeys.REGISTER_KEY(Tags.PATIENT_TAG),
                JsonSerializer.Serialize(request),
                cancellationToken);

            if (result)
            {
                //var newEvent = new PatientRegisteredEvent();
                // Do something
            }
        }
    }
}
