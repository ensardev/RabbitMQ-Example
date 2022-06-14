using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;

namespace RabbitMQ.ConsoleSubscriber
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
            //channel.QueueDeclare("hello-queue", true, false, false, null);

            var queueName = "direct-queue-Critical";
            
            channel.BasicQos(0, 1, false);

            //Create consumer
            var consumer = new EventingBasicConsumer(channel);
            Console.WriteLine("Waiting for logs...");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine(" [x] Log : {0}", message);

                //File.AppendAllText("logs-critical.txt", message + Environment.NewLine);

                channel.BasicAck(e.DeliveryTag, false);
            };
            
            channel.BasicConsume(queueName, false, consumer);

            Console.ReadLine();
        }
    }
}
