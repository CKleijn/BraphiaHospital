using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PatientManagement.Common.Annotations;
using System.Data.SqlClient;

namespace PatientManagement.Infrastructure.Persistence.Stores
{
    public class EventStore
        : IEventStore
    {
        public async Task<IEnumerable<TEntity>> GetAllEventsByEvent<TEvent, TEntity>(CancellationToken cancellationToken)
        {
            var events = new List<TEntity>();

            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "SELECT Payload FROM Events WHERE Type = @Entity";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Entity", typeof(TEvent).Name);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var entity = TranslatePayload<TEntity>(reader.GetString(0));
                        events.Add(entity);
                    }
                }

                return events;
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_READ_ERROR);
            }
        }

        public async Task<bool> AddEvent(string type, string? payload, CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "INSERT INTO Events (Type, Payload) VALUES (@Type, @Payload)";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Type", type);
                command.Parameters.AddWithValue("@Payload", payload);

                await command.ExecuteNonQueryAsync(cancellationToken);

                return true;
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_INSERT_ERROR);
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
