using MassTransit;
using MassTransitMessage;

namespace Receiver2;

static class Program
{
    static async Task Main()
    {
        Console.WriteLine("MassTransit RabbitMQ Receiver2");

        IBusControl busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host("localhost", "/", h =>
            {
                h.Username("guest");
                h.Password("guest");
                h.ConnectionName("RabbitMQ Receiver2");
            });

            cfg.ReceiveEndpoint("mt.simplemessage.consumer.2", epc =>
            {
                //epc.Consumer<SimpleConsumer>();   //This and Handler WILL run concurrently
                epc.PrefetchCount = 1;
                epc.UseConcurrencyLimit(1);
                epc.Handler<ISimpleMessage>(context =>
                {
                    Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: Message received = {context.Message.Text}");
                    return Task.CompletedTask;
                });

            });
            cfg.Message<ISimpleMessage>(x => x.SetEntityName("mt.simplemessage"));
        });

        //Start to listen
        await busControl.StartAsync(); // This is non-blocking
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}

/* Not used in this example
public class SimpleConsumer : IConsumer<ISimpleMessage>
{
    public async Task Consume(ConsumeContext<ISimpleMessage> context)
    {
        Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: Message received = {context.Message.Text}");
        await Task.CompletedTask;
    }
}*/