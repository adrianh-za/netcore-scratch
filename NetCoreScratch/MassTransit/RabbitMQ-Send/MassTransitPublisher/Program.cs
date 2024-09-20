using MassTransit;
using MassTransitMessage;
using static System.Net.Mime.MediaTypeNames;

namespace Sender;

static class Program
{
    static async Task Main()
    {
        Console.WriteLine("MassTransit RabbitMQ Publisher");
        await Task.Delay(1000); // Wait for the receiver to start

        IBusControl busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host("localhost", "/", x =>
            {
                x.Username("guest");
                x.Password("guest");
                x.ConnectionName("RabbitMQ Publisher");
            });
            //cfg.Message<SimpleMessage>(x => x.SetEntityName("mt-rmq-exchange"));
            /*
            cfg.Publish<SimpleMessage>(x =>
            {
                x.ExchangeType = "direct";
            });
            cfg.Send<SimpleMessage>(x =>
            {
                x.UseRoutingKeyFormatter(context => context.Message.Text);
            });*/
        });

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
            await busControl.Publish(new SimpleMessage { Text = text });
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: Message sent = {text}");
            counter++;
        }

        //This needs to be called, but for demo we are ok
        await busControl.StopAsync();
    }
}