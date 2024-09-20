using MassTransit;
using MassTransitMessage;

namespace Sender;

static class Program
{
    static async Task Main()
    {
        Console.WriteLine("MassTransit RabbitMQ Sender");
        await Task.Delay(1000); // Wait for the receiver to start

        IBusControl busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host("localhost", "/", x =>
            {
                x.Username("guest");
                x.Password("guest");
                x.ConnectionName("RabbitMQ Sender");
            });
        });

        var sendToUri = new Uri("rabbitmq://localhost/mt-rmq-queue");
        var endpoint = await busControl.GetSendEndpoint(sendToUri);
        await busControl.StartAsync(); // This is non-blocking


        var random = new Random();
        var counter = 0;
        while (true)
        {
            //Randomise the delay
            var delay = random.Next(50, 2000);
            Thread.Sleep(delay);

            //Send the message
            var text = $"[{counter}] Hello world!";
            await endpoint.Send(new SimpleMessage { Text = text});
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: Message sent = {text}");
            counter++;
        }

        //This needs to be called, but for demo we are ok
        await busControl.StopAsync();
    }
}