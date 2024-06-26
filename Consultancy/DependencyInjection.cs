using System.Reflection;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Consultancy.Infrastructure.MessageBus.Implementations;
using Consultancy.Infrastructure.MessageBus.Interfaces;
using Consultancy.Infrastructure.Persistence.Contexts;
using Consultancy.Infrastructure.Persistence.Stores;
using Consultancy.Common.Mappers;
using Consultancy.Common.Interfaces;

namespace Consultancy
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
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddScoped<IApiClient, ApiClient>();
            builder.Services.AddScoped<IEventStore, EventStore>();
            builder.Services.AddScoped<IEventRouter, EventRouter>();
            builder.Services.AddScoped<IProducer, Producer>();
            builder.Services.AddScoped<IConsumer, Consumer>();
            builder.Services.AddScoped<IQuestionMapper, QuestionMapper>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
    }
}
