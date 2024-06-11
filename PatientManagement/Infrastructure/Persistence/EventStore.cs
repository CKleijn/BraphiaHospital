using MediatR;
using PatientManagement.Common.Annotations;
using System.Data.SqlClient;

namespace PatientManagement.Infrastructure.Persistence
{
    public class EventStore
        : IEventStore
    {
        public async Task<IEnumerable<INotification>> GetAllEvents(CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "SELECT * FROM Events";

                using SqlCommand command = new(query, connection);
                using SqlDataReader reader = command.ExecuteReader();

                var events = new List<INotification>();

                while (await reader.ReadAsync())
                {
                    var eventKey = reader.GetString(1);
                    var eventValue = reader.GetString(2);
                    Console.WriteLine(eventKey);
                    Console.WriteLine(eventValue);
                    //events.Add(eventValue);
                }

                await connection.CloseAsync();

                return events;
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_READ_ERROR);
            }
        }

        public async Task<bool> AddEvent(string eventKey, string? eventValue, CancellationToken cancellationToken)
        {
            try
            {
                using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

                await connection.OpenAsync(cancellationToken);

                string query = "INSERT INTO Events (EventKey, EventValue) VALUES (@EventKey, @EventValue)";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@EventKey", eventKey);
                command.Parameters.AddWithValue("@EventValue", eventValue);

                await command.ExecuteNonQueryAsync(cancellationToken);

                await connection.CloseAsync();

                return true;
            }
            catch (Exception)
            {
                throw new Exception(Errors.SQL_INSERT_ERROR);
            }
        }
    }
}
