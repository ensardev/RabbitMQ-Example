using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.ConsolePublisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //RabbitMQ connection factory create
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://tvfwnuej:WeJUaYAELRx62MP8yMJcEiNNqWEAYETS@sparrow.rmq.cloudamqp.com/tvfwnuej");

            //Create connection
            using var connection = factory.CreateConnection();

            //Create channel
            var channel = connection.CreateModel();

            //Create queue
            channel.QueueDeclare("hello-queue", true, false, false, null);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                //Publish message
                string message = $"Hello World! {x}";
                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("", "hello-queue", null, messageBody);

                Console.WriteLine("Message send.");
            });

            Console.ReadLine();
        }
    }
}
