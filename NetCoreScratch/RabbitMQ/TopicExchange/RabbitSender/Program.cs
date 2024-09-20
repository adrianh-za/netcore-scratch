﻿using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest",
    VirtualHost = "/",
    Port = 5672,
    ClientProvidedName = "RabbitMQ Sender", //Important to classify the message sender for debugging purposes
    //Uri = new Uri("amqp://guest:guest@localhost:5672") //A shorter way to set the connection parameters
};

const string ExchangeName = "DemoTopicExchange";
var random = new Random();

//Setup the connection and channel
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic);

    var counter = 0;
    while (true)
    {
        //Randomise the delay
        var delay = random.Next(50, 2000);
        Thread.Sleep(delay);

        //Send a new message
        var topic = GetRandomTopic();
        var text = $"[{counter}] [{topic}] Hello world!";
        var message = Encoding.UTF8.GetBytes(text);
        channel.BasicPublish(ExchangeName, topic, null, message);
        Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: Message sent = {text} ");

        counter++;
    }
}

string GetRandomTopic()
{
    var random = new Random();
    return random.Next(2) == 0 ? "topic.AAAA" : "topic.BBBB";
}