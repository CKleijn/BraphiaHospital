﻿using MediatR;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client.Events;
using Newtonsoft.Json.Linq;
using Consultancy.Features.ConsultFeature.CreateConsult.Event;
using Consultancy.Common.Entities;
using Consultancy.Common.Interfaces;
using Consultancy.Infrastructure.MessageBus.Interfaces;
using Consultancy.Features.ConsultFeature.UpdateQuestion.Event;

namespace Consultancy.Infrastructure.MessageBus.Implementations
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
                case nameof(ConsultCreatedEvent):
                    await publisher.Publish(JsonConvert.DeserializeObject<ConsultCreatedEvent>(payload)!);
                    break;
                case nameof(QuestionUpdatedEvent):
                    await publisher.Publish(new QuestionUpdatedEvent(TranslatePayload<Question>(payload)));
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
