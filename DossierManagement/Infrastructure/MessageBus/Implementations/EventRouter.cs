using MediatR;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client.Events;
using Newtonsoft.Json.Linq;
using DossierManagement.Infrastructure.MessageBus.Interfaces;
using DossierManagement.Common.Interfaces;
using DossierManagement.Features.Dossier.CreateDossier.Event;
using DossierManagement.Features.Dossier;

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
                case nameof(PatientRegisteredEvent):
                    await publisher.Publish(new PatientRegisteredEvent(TranslatePayload<Patient>(payload)));
                    break;
                case nameof(DossierCreatedEvent):
                    await publisher.Publish(new DossierCreatedEvent(TranslatePayload<Dossier>(payload)));
                    break;
            }
        }

        private T TranslatePayload<T>(string payload)
            where T : IEntity
        {
            var jsonObject = JObject.Parse(payload);
            var entityPayload = (jsonObject[typeof(T).Name]?.ToString())
                ?? throw new ArgumentException($"Payload does not contain an entity of type {typeof(T).Name}");

            return JsonConvert.DeserializeObject<T>(entityPayload)!;
        }
    }
}
