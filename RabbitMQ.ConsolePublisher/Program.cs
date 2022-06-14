using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.ConsolePublisher
{
    internal class Program
    {
        public enum Logs
        {
            Critical = 1,
            Error = 2,
            Warning = 3,
            Info = 4
        }

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

            channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

            //Create queue for DirectExchange
            Enum.GetNames(typeof(Logs)).ToList().ForEach(x =>
            {
                var queueName = $"direct-queue-{x}";
                var routeKey = $"route-{x}";
                
                channel.QueueDeclare(queueName, true, false, false);
                channel.QueueBind(queueName, "logs-direct",routeKey, null);

            });

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                Logs log = (Logs)new Random().Next(1, 4);

                var routeKey = $"route-{log}";
                
                //Publish message
                string message = $"Log type = {log}";
                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("logs-direct", routeKey, null, messageBody);

                Console.WriteLine($"Log saved. Detail = {message}");
            });

            Console.ReadLine();
        }
    }
}
