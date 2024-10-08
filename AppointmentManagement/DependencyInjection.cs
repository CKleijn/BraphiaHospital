﻿using System.Reflection;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AppointmentManagement.Infrastructure.MessageBus.Implementations;
using AppointmentManagement.Infrastructure.MessageBus.Interfaces;
using AppointmentManagement.Infrastructure.Persistence.Contexts;
using AppointmentManagement.Infrastructure.Persistence.Stores;
using AppointmentManagement.Common.Interfaces;
using AppointmentManagement.Common.Mappers;
using System.Net.Http;

namespace AppointmentManagement
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

            builder.Services.AddScoped<IReferralMapper, ReferralMapper>();
            builder.Services.AddScoped<IAppointmentMapper, AppointmentMapper>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
    }
}