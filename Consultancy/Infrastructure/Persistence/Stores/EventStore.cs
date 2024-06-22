using Consultancy.Common.Abstractions;
using Consultancy.Common.Annotations;
using Consultancy.Common.Entities;
using Consultancy.Features.ConsultFeature.CreateConsult.RabbitEvent;
using Consultancy.Features.ConsultFeature.UpdateNotes.RabbitEvent;
using Consultancy.Features.ConsultFeature.UpdateQuestions.RabbitEvent;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Consultancy.Infrastructure.Persistence.Stores
{
    public class EventStore
        : IEventStore
    {
        public async Task<IEnumerable<Event>> GetAllEventsByAggregateId(
            Guid aggregateId,
            CancellationToken cancellationToken)
        {
            try
            {
                var events = new List<Event>();

                using SqlConnection connection = new(ConfigurationHelper.GetEventConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "SELECT Type, Payload, Version FROM Events WHERE AggregateId = @AggregateId ORDER BY Version ASC";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@AggregateId", aggregateId);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var type = reader.GetString(0);
                        var payload = reader.GetString(1);
                        var version = reader.GetInt32(2);

                        switch (type)
                        {
                            case nameof(ConsultCreatedEvent):
                                events.Add(new ConsultCreatedEvent(JsonConvert.DeserializeObject<Consult>(payload)!)
                                {
                                    AggregateId = aggregateId,
                                    Type = type,
                                    Payload = payload,
                                    Version = version,
                                });
                                break;
                            case nameof(DossierConsultAppendedEvent):
                                events.Add(new DossierConsultAppendedEvent(JsonConvert.DeserializeObject<Consult>(payload)!)
                                {
                                    AggregateId = aggregateId,
                                    Type = type,
                                    Payload = payload,
                                    Version = version,
                                });
                                break;
                            case nameof(ConsultSurveyFilledInEvent):
                                events.Add(new ConsultSurveyFilledInEvent(JsonConvert.DeserializeObject<ICollection<Question>>(payload)!)
                                {
                                    AggregateId = aggregateId,
                                    Type = type,
                                    Payload = payload,
                                    Version = version,
                                });
                                break;
                        }
                    }
                }

                return events;
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_READ_ERROR);
            }
        }

        public async Task<bool> EventByAggregateIdExists(
            Guid aggregateId,
            CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetEventConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "SELECT COUNT(1) FROM Events WHERE AggregateId = @AggregateId";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@AggregateId", aggregateId);

                return ((int)await command.ExecuteScalarAsync(cancellationToken) > 0);
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_READ_ERROR);
            }
        }

        public async Task<bool> AppointmentAlreadyHasConsult(
            Guid appointmentId,
            CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetEventConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "SELECT COUNT(1) FROM Events WHERE JSON_VALUE(Payload, '$.AppointmentId') = @AppointmentId;";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@AppointmentId", appointmentId);

                return ((int)await command.ExecuteScalarAsync(cancellationToken) > 0);
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_READ_ERROR);
            }
        }

        public async Task<bool> AddEvent(
            Event @event,
            CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetEventConnectionString());

                await connection.OpenAsync(cancellationToken);

                string validationQuery = "SELECT COUNT(1) FROM Events WHERE AggregateId = @AggregateId AND Type = @Type AND Version = @Version";

                using SqlCommand validationCommand = new(validationQuery, connection);
                validationCommand.Parameters.AddWithValue("@AggregateId", @event.AggregateId);
                validationCommand.Parameters.AddWithValue("@Type", @event.Type);
                validationCommand.Parameters.AddWithValue("@Version", @event.Version);

                if ((int)await validationCommand.ExecuteScalarAsync(cancellationToken) > 0) return false;

                string query = "INSERT INTO Events (AggregateId, Type, Payload, Version) VALUES (@AggregateId, @Type, @Payload, @Version)";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@AggregateId", @event.AggregateId);
                command.Parameters.AddWithValue("@Type", @event.Type);
                command.Parameters.AddWithValue("@Payload", @event.Payload);
                command.Parameters.AddWithValue("@Version", @event.Version);

                await command.ExecuteNonQueryAsync(cancellationToken);

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(Errors.SQL_INSERT_ERROR);
            }
        }
    }
}
