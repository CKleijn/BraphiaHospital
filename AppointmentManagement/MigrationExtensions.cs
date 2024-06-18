using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using AppointmentManagement.Infrastructure.Persistence.Contexts;

namespace AppointmentManagement
{
    public static class MigrationExtensions
    {
        public static void ApplyDatabaseMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            bool databaseExists = dbContext.Database.CanConnect();

            if(databaseExists)
            {
                dbContext.Database.Migrate();
            } 
            else
            {
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
            }
        }

        public static void ApplyEventStoreMigrations(this IApplicationBuilder app)
        {
            using SqlConnection connection = new(ConfigurationHelper.GetGlobalConnectionString());

            connection.Open();

            string createDatabaseScript = @"
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'BraphiaHospitalAppointmentManagementEventStore')
                BEGIN
                    CREATE DATABASE BraphiaHospitalAppointmentManagementEventStore;
                END;";
            ExecuteSqlCommand(connection, createDatabaseScript);

            string useDatabaseScript = "USE BraphiaHospitalAppointmentManagementEventStore;";
            ExecuteSqlCommand(connection, useDatabaseScript);

            string createTableScript = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Events')
                BEGIN
                    CREATE TABLE Events (
                        ID INT PRIMARY KEY IDENTITY(1,1),
                        Type NVARCHAR(100) NOT NULL,
                        Payload NVARCHAR(MAX) NULL,
                        Version INT NULL,
                        CreatedAt DATETIME DEFAULT GETDATE()
                    );
                END;";
            ExecuteSqlCommand(connection, createTableScript);
        }

        private static void ExecuteSqlCommand(
            SqlConnection connection,
            string query)
        {
            using SqlCommand command = new(query, connection);
            command.ExecuteNonQuery();
        }
    }
}
