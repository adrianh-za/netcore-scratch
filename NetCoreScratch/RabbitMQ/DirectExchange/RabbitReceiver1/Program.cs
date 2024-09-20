using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest",
    VirtualHost = "/",
    Port = 5672,
    ClientProvidedName = "RabbitMQ Receiver 1", //Important to classify the message sender for debugging purposes
    //Uri = new Uri("amqp://guest:guest@localhost:5672") //A shorter way to set the connection parameters
};

const string ExchangeName = "DemoExchange";
const string QueueName = "DemoQueue";
const string RouteKey = "demo-routing-key";

//Setup the connection and channel
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
    channel.QueueDeclare(QueueName, false, false, false, null);
    channel.QueueBind(QueueName, ExchangeName, RouteKey);
    channel.BasicQos(0, 1, false);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, eventArgs) =>
    {
        Thread.Sleep(1000); //Simulate a slow consumer
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        channel.BasicAck(eventArgs.DeliveryTag, false);
        Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: Message received = {message}");
    };

    string consumerTag = channel.BasicConsume(QueueName, false, consumer);
    Console.ReadKey();
    channel.BasicCancel(consumerTag);
}