﻿namespace AppointmentManagement
{
    public class ConfigurationHelper
    {
        public static string GetConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            return configuration.GetConnectionString("EventStoreConnectionString")!;
        }
    }
}