using Consultancy.Models;
using System.Data.SqlClient;

namespace Consultancy.Utils
{
    public static class DatabaseEventOperations
    {
        public static List<Event> ReadEvents()
        {
            List<Event> events = [];

            using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

            connection.Open();

            string query = "SELECT * FROM Event";

            using SqlCommand command = new(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Event eventObj = new(reader.GetInt32(0).ToString(), reader.GetString(1));
                events.Add(eventObj);
            }

            connection.Close();
            return events;
        }

        public static void AddEvent(string eventValue)
        {
            using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

            connection.Open();

            string query = "INSERT INTO Event (Event) VALUES (@EventValue)";

            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@EventValue", eventValue);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}