using Consultancy.Infrastructure.MessageBus.Interfaces;

namespace Consultancy
{
    public static class RabbitMqExtensions
    {
        public static void RunConsumer(this IApplicationBuilder app)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                using var scope = app.ApplicationServices.CreateScope();
                var consumer = scope.ServiceProvider.GetRequiredService<IConsumer>();
                consumer.Consume();
            })
            .Start();
        }
    }
}
