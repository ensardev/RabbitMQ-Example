using RabbitMQ.Client;
using System;
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

            //Publish message
            string message = "Hello World!";
            var messageBody = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("", "hello-queue", null, messageBody);

            Console.WriteLine("Message send.");

            Console.ReadLine();
        }
    }
}
