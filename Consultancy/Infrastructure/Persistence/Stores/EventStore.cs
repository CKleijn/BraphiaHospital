using Consultancy.Common.Annotations;
using System.Data.SqlClient;

namespace Consultancy.Infrastructure.Persistence.Stores
{
    public class EventStore
        : IEventStore
    {
        public async Task<bool> AddEvent(string type, string? payload, CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetEventConnectionString());

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
    }
}
