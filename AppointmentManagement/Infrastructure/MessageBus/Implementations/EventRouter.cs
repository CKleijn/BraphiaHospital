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
using AppointmentManagement.Features.StaffMemberFeature.CreateStaffMember.Event;
using AppointmentManagement.Features.StaffMemberFeature.UpdateStaffMember.Event;
using AppointmentManagement.Features.HospitalFacilityFeature.CreateHospitalFacility.Event;
using AppointmentManagement.Features.HospitalFacilityFeature.UpdateHospitalFacility.Event;
using AppointmentManagement.Common.Aggregates;

namespace AppointmentManagement.Infrastructure.MessageBus.Implementations
{
    public sealed class EventRouter(IPublisher publisher)
        : IEventRouter
    {
        public async Task RouteEvents(BasicDeliverEventArgs eventArgs)
        {
            var body = eventArgs.Body.ToArray();
            var payload = Encoding.UTF8.GetString(body);

            string eventName = "";

            if (eventArgs.BasicProperties.Headers?.TryGetValue("EventName", out var eventObj) == true)
            {
                eventName = Encoding.UTF8.GetString((byte[])eventObj);
            }
            else
            {
                var routingKeyParts = eventArgs.RoutingKey.Split('.');
                if (routingKeyParts.Length >= 2)
                {
                    eventName = $"{routingKeyParts[0]}{routingKeyParts[1]}Event";
                }
            }

            if (!string.IsNullOrEmpty(eventName))
                await HandlePublishEvent(eventName, payload);
        }

        private async Task HandlePublishEvent(string eventName, string payload)
        {
            switch (eventName)
            {
                //Referral
                case nameof(ReferralCreatedEvent):
                    await publisher.Publish(new ReferralCreatedEvent(TranslatePayloadToEntity<Referral>(payload)));
                    break;
                //Appointment
                case nameof(AppointmentScheduledEvent):
                    await publisher.Publish(JsonConvert.DeserializeObject<AppointmentScheduledEvent>(payload)!);
                    break;
                case nameof(AppointmentRescheduledEvent):
                    await publisher.Publish(JsonConvert.DeserializeObject<AppointmentRescheduledEvent>(payload)!);
                    break;
                case nameof(AppointmentArrivalUpdatedEvent):
                    await publisher.Publish(JsonConvert.DeserializeObject<AppointmentArrivalUpdatedEvent>(payload)!);
                    break;

                //External
                case nameof(StaffCreatedEvent):
                    await publisher.Publish(new StaffCreatedEvent(JsonConvert.DeserializeObject<StaffMember>(payload)!));
                    break;
                case nameof(StaffUpdatedEvent):
                    await publisher.Publish(JsonConvert.DeserializeObject<StaffUpdatedEvent>(payload)!);
                    break;
                case nameof(HospitalFacilityCreatedEvent):
                    await publisher.Publish(new HospitalFacilityCreatedEvent(JsonConvert.DeserializeObject<HospitalFacility>(payload)!));
                    break;
                case nameof(HospitalFacilityUpdatedEvent):
                    await publisher.Publish(JsonConvert.DeserializeObject<HospitalFacilityUpdatedEvent>(payload)!);
                    break;
            }
        }

        private T TranslatePayloadToEntity<T>(string payload)
            where T : AggregateRoot
        {
            var jsonObject = JObject.Parse(payload);
            var entityPayload = (jsonObject[typeof(T).Name]?.ToString())
                ?? throw new ArgumentException($"Payload does not contain an entity of type {typeof(T).Name}");

            return JsonConvert.DeserializeObject<T>(entityPayload)!;
        }
    }
}
