using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
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
            var randomQueueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(randomQueueName, "logs-fanout", "", null);

            //Create queue
            //channel.QueueDeclare("hello-queue", true, false, false, null);

            channel.BasicQos(0, 1, false);

            //Create consumer
            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(randomQueueName, false, consumer);

            Console.WriteLine("Waiting for logs...");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine(" [x] Log : {0}", message);

                channel.BasicAck(e.DeliveryTag, false);
            };


            Console.ReadLine();
        }
    }
}
