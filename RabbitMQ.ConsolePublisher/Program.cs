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
            //Comment for Exchange example
            //channel.QueueDeclare("hello-queue", true, false, false, null); 

            channel.ExchangeDeclare("logs-fanout",durable:true,type:ExchangeType.Fanout);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                //Publish message
                string message = $"Log {x}";
                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("logs-fanout", "", null, messageBody);

                Console.WriteLine("Message send.");
            });

            Console.ReadLine();
        }
    }
}
