﻿using MassTransit;
using MassTransitMessage;

namespace Receiver1;

static class Program
{
    static async Task Main()
    {
        Console.WriteLine("MassTransit RabbitMQ Receiver1");

        IBusControl busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host("localhost", "/", h =>
            {
                h.Username("guest");
                h.Password("guest");
                h.ConnectionName("RabbitMQ Receiver1");
            });

            cfg.ReceiveEndpoint("mt-rmq-queue-1", e =>
            {
                e.Consumer<SimpleConsumer>();
                e.PrefetchCount = 1;
                e.UseConcurrencyLimit(1);
            });
        });

        //Start to listen
        //await busControl.StartAsync(); // This is non-blocking
        busControl.Start();
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}

public class SimpleConsumer : IConsumer<SimpleMessage>
{
    public async Task Consume(ConsumeContext<SimpleMessage> context)
    {
        Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: Message received = {context.Message.Text}");
        await Task.CompletedTask;
    }
}