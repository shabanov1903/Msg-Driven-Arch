using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Messaging
{
    public class Consumer : IDisposable
    {
        private readonly string _queueName; // query name
        private readonly string _hostName; // hostname

        private readonly IConnection _connection;
        private readonly IModel _channel;
        
        public Consumer(string queueName = "", string hostName = "localhost")
        {
            _queueName = queueName;
            _hostName = "rattlesnake.rmq.cloudamqp.com"; // set hostName;
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = 5672,
                UserName = "wqxcpmsj",
                Password = "5ycB-owHLnQbJmWGDI1Iq7eAMcuOZile",
                VirtualHost = "wqxcpmsj"

            };
            _connection = factory.CreateConnection(); // create app
            _channel = _connection.CreateModel();
        }

        public void Receive (EventHandler<BasicDeliverEventArgs> receiveCallback)
        {
            _channel.ExchangeDeclare(exchange: "direct_exchange", type: ExchangeType.Fanout); // declare direct
            
            _channel.QueueDeclare(queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null); // declare query

            _channel.QueueBind(queue: _queueName,
                exchange: "direct_exchange",
                routingKey: _queueName); // bind

            var consumer = new EventingBasicConsumer(_channel); // create customer
            consumer.Received += receiveCallback; // create callback

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer); // start process
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}