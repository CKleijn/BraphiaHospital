using MediatR;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client.Events;
using Newtonsoft.Json.Linq;
using DossierManagement.Infrastructure.MessageBus.Interfaces;
using DossierManagement.Features.Dossier;
using DossierManagement.Events.ConsultAppended;
using DossierManagement.Events.ResultAppended;
using DossierManagement.Events.LabTestResultAvailable;
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

        private async Task HandlePublishEvent(
            string eventName, 
            string payload)
        {
            switch (eventName)
            {
                case nameof(DossierConsultAppendedEvent):
                    await publisher.Publish(new DossierConsultAppendedEvent(TranslatePayload<Dossier>(payload)));
                    break;
                case nameof(DossierCreatedEvent):
                    await publisher.Publish(new DossierCreatedEvent(TranslatePayload<Dossier>(payload)));
                    break;
                case nameof(LaboratoryTestResultAvailableEvent):
                    await publisher.Publish(new LaboratoryTestResultAvailableEvent(TranslatePayload<Guid>(payload), TranslatePayload<Result>(payload)));
                    break;
                case nameof(DossierMedicationPrescribedEvent):
                    await publisher.Publish(new DossierMedicationPrescribedEvent(TranslatePayload<Dossier>(payload)));
                    break;
                case nameof(PatientRegisteredEvent):
                    await publisher.Publish(new PatientRegisteredEvent(TranslatePayload<Patient>(payload)));
                    break;
                case nameof(DossierResultAppendedEvent):
                    await publisher.Publish(new DossierResultAppendedEvent(TranslatePayload<Dossier>(payload)));
                    break;
            }
        }

        private T TranslatePayload<T>(string payload)
        {
            var jsonObject = JObject.Parse(payload);
            var entityPayload = (jsonObject[typeof(T).Name]?.ToString())
                ?? throw new ArgumentException($"Payload does not contain an entity of type {typeof(T).Name}");

            return JsonConvert.DeserializeObject<T>(entityPayload)!;
        }
    }
}
