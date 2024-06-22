using MediatR;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client.Events;
using Newtonsoft.Json.Linq;
using DossierManagement.Infrastructure.MessageBus.Interfaces;
using DossierManagement.Events.ConsultAppended;
using DossierManagement.Events.DossierCreated;
using DossierManagement.Events.PatientRegistered;
using DossierManagement.Events.MedicationPrescribed;

namespace DossierManagement.Infrastructure.MessageBus.Implementations
{
    public sealed class EventRouter(IPublisher publisher)
        : IEventRouter
    {
        public async Task RouteEvents(BasicDeliverEventArgs eventArgs)
        {
            var body = eventArgs.Body.ToArray();
            var payload = Encoding.UTF8.GetString(body);

            if (eventArgs.BasicProperties.Headers.TryGetValue("EventName", out var eventObj))
            {
                var eventName = Encoding.UTF8.GetString((byte[])eventObj);

                await HandlePublishEvent(eventName, payload);
            }
        }

        private async Task HandlePublishEvent(string eventName, string payload)
        {
            switch (eventName)
            {
                case nameof(DossierConsultAppendedEvent):
                    await publisher.Publish(JsonConvert.DeserializeObject<DossierConsultAppendedEvent>(payload)!);
                    break;
                case nameof(DossierCreatedEvent):
                    await publisher.Publish(JsonConvert.DeserializeObject<DossierCreatedEvent>(payload)!);
                    break;
                case nameof(DossierMedicationPrescribedEvent):
                    await publisher.Publish(JsonConvert.DeserializeObject<DossierMedicationPrescribedEvent>(payload)!);
                    break;
                case nameof(PatientRegisteredEvent):
                    await publisher.Publish(JsonConvert.DeserializeObject<PatientRegisteredEvent>(payload)!);
                    break;
            }
        }
    }
}
