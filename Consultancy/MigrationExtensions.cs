using Microsoft.EntityFrameworkCore;
using Consultancy.Infrastructure.Persistence.Contexts;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Consultancy
{
    public static class MigrationExtensions
    {
        public static void ApplyDatabaseMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var anyTablesExist = dbContext.Database.GetService<IRelationalDatabaseCreator>()
                .Exists() && dbContext.GetService<IRelationalDatabaseCreator>()
                .HasTables();

            if (!anyTablesExist)
                dbContext.Database.Migrate();
        }

        public static void ApplyEventStoreMigrations(this IApplicationBuilder app)
        {
            using SqlConnection connection = new(ConfigurationHelper.GetGlobalConnectionString());

            connection.Open();

            string createDatabaseScript = @"
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'BraphiaHospitalConsultancyEventStore')
                BEGIN
                    CREATE DATABASE BraphiaHospitalConsultancyEventStore;
                END;";
            ExecuteSqlCommand(connection, createDatabaseScript);

            string useDatabaseScript = "USE BraphiaHospitalConsultancyEventStore;";
            ExecuteSqlCommand(connection, useDatabaseScript);

            string createTableScript = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Events')
                BEGIN
                    CREATE TABLE Events (
                    ID INT PRIMARY KEY identity(1,1),
                    AggregateId UNIQUEIDENTIFIER NOT NULL,
                    Type NVARCHAR(100) NOT NULL,
                    Payload NVARCHAR(MAX) NULL,
                    Version INT NOT NULL,
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
