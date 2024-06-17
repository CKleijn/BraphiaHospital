namespace Consultancy
{
    public static class ConfigurationHelper
    {
        public static string GetApplicationConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            return configuration.GetConnectionString("ApplicationConnectionString")!;
        }

        public static string GetGlobalConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            return configuration.GetConnectionString("GlobalEventStoreConnectionString")!;
        }

        public static string GetEventConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            return configuration.GetConnectionString("EventStoreConnectionString")!;
        }
    }
}
