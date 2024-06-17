using MediatR;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client.Events;
using Newtonsoft.Json.Linq;
using AppointmentManagement.Common.Interfaces;
using AppointmentManagement.Infrastructure.MessageBus.Interfaces;
using AppointmentManagement.Common.Entities;
using AppointmentManagement.Features.ReferralFeature.CreateReferral.Event;
using AppointmentManagement.Features.AppointmentFeature.ScheduleAppointment.Event;
using AppointmentManagement.Features.AppointmentFeature.UpdatePatientArrival.Event;

namespace AppointmentManagement.Infrastructure.MessageBus.Implementations
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
                //Referral
                case nameof(ReferralCreatedEvent):
                    await publisher.Publish(new ReferralCreatedEvent(TranslatePayload<Referral>(payload)));
                    break;
                //Appointment
                case nameof(AppointmentScheduledEvent):
                    await publisher.Publish(new AppointmentScheduledEvent(TranslatePayload<Appointment>(payload)));
                    break;
                case nameof(AppointmentRescheduledEvent):
                    await publisher.Publish(new AppointmentRescheduledEvent(TranslatePayload<Appointment>(payload)));
                    break;
                case nameof(PatientArrivalUpdatedEvent):
                    await publisher.Publish(new PatientArrivalUpdatedEvent(TranslatePayload<Appointment>(payload)));
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
