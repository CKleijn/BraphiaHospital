using System.Reflection;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PatientManagement.Features.Patient;
using PatientManagement.Features.Patient._Interfaces;
using PatientManagement.Infrastructure.MessageBus.Implementations;
using PatientManagement.Infrastructure.MessageBus.Interfaces;
using PatientManagement.Infrastructure.Persistence.Contexts;
using PatientManagement.Infrastructure.Persistence.Stores;

namespace PatientManagement
{
    public static class DependencyInjection
    {
        public static void Configure(WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ApplicationConnectionString")));

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Services.AddCarter();
            builder.Services.AddScoped<IEventStore, EventStore>();
            builder.Services.AddScoped<IEventRouter, EventRouter>();
            builder.Services.AddScoped<IProducer, Producer>();
            builder.Services.AddScoped<IConsumer, Consumer>();
            builder.Services.AddScoped<IPatientMapper, PatientMapper>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
    }
}
