using Carter;
using PatientManagement;

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
    app.ApplyDatabaseMigrations();
    app.ApplyEventStoreMigrations();
}

app.UseHttpsRedirection();
app.UseRouting();

app.RunConsumer();

app.Run();