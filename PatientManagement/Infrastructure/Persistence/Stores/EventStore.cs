using PatientManagement.Common.Abstractions;
using PatientManagement.Common.Annotations;
using PatientManagement.Common.Entities;
using System.Data.SqlClient;

namespace PatientManagement.Infrastructure.Persistence.Stores
{
    public class EventStore
        : IEventStore
    {
        public async Task<IEnumerable<StoredEvent>> GetAllEventsByAggregateId(
            Guid aggregateId, 
            CancellationToken cancellationToken)
        {
            try
            {
                var events = new List<StoredEvent>();

                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

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

                        events.Add(new StoredEvent(aggregateId, type, payload, version));
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
                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "SELECT COUNT(1) FROM Events WHERE AggregateId = @AggregateId";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@AggregateId", aggregateId);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_READ_ERROR);
            }
        }

        public async Task<bool> BSNExists(
            string BSN,
            CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "SELECT COUNT(1) FROM Events WHERE JSON_VALUE(Payload, '$.Patient.BSN') = @BSN;";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@BSN", BSN);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        return true;
                    }
                }

                return false;
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
                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                string validationQuery = "SELECT COUNT(1) FROM Events WHERE AggregateId = @AggregateId AND Type = @Type AND Version = @Version";

                using SqlCommand validationCommand = new(validationQuery, connection);
                validationCommand.Parameters.AddWithValue("@AggregateId", @event.AggregateId);
                validationCommand.Parameters.AddWithValue("@Type", @event.Type);
                validationCommand.Parameters.AddWithValue("@Version", @event.Version);

                using (var reader = await validationCommand.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        return false;
                    }
                }

                string query = "INSERT INTO Events (AggregateId, Type, Payload, Version) VALUES (@AggregateId, @Type, @Payload, @Version)";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@AggregateId", @event.AggregateId);
                command.Parameters.AddWithValue("@Type", @event.Type);
                command.Parameters.AddWithValue("@Payload", @event.Payload);
                command.Parameters.AddWithValue("@Version", @event.Version);

                await command.ExecuteNonQueryAsync(cancellationToken);

                return true;
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_INSERT_ERROR);
            }
        }
    }
}
