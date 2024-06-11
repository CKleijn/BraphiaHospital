using Carter;
using PatientManagement;
using PatientManagement.Infrastructure.MessageBus;

var builder = WebApplication.CreateBuilder(args);

DependencyInjection.Configure(builder);

var app = builder.Build();

var apiGroup = app
    .MapGroup("/api");

apiGroup.MapCarter();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

Task.Run(() => Consumer.Consume());

app.Run();