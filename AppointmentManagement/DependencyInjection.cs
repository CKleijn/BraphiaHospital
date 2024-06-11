using System.Reflection;
using Carter;
using AppointmentManagement.Infrastructure.Persistence;

namespace AppointmentManagement
{
    public class DependencyInjection
    {
                public static void Configure(WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            builder.Services.AddCarter();
            builder.Services.AddScoped<IEventStore, EventStore>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
    }
}