using System.Data.SqlClient;

namespace Consultancy.Utils
{
    public static class DatabaseEventOperations
    {
        public static void ReadEvents()
        {
            using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

            connection.Open();

            string query = "SELECT * FROM Events";

            using SqlCommand command = new(query, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) Console.WriteLine(reader.GetString(1));

            connection.Close();
        }

        public static void AddEvent(string eventValue)
        {
            using SqlConnection connection = new(ConfigurationHelper.GetConnectionString());

            connection.Open();

            string query = "INSERT INTO Events (Event) VALUES (@EventValue)";

            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@EventValue", eventValue);

            connection.Close();
        }
    }
}