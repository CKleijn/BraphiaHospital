namespace AppointmentManagement
{
    public static class ConfigurationHelper
    {
        public static string GetConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            return configuration.GetConnectionString("EventStoreConnectionString")!;
        }

        public static string GetGlobalConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            return configuration.GetConnectionString("GlobalEventStoreConnectionString")!;
        }

        public static string GetPatientManagementServiceConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            return configuration.GetConnectionString("PatientManagementServiceConnectionString")!;
        }
    }
}
