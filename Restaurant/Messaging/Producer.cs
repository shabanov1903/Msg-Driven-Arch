using System.Collections;
using System.Text;
using RabbitMQ.Client;

namespace Messaging
{
    public class Producer
    {
        private readonly string _queueName;
        private readonly string _hostName;

        public Producer(string queueName, string hostName = "localhost")
        {
            _queueName = queueName;
            _hostName = "rattlesnake.rmq.cloudamqp.com"; // hostName
        }

        public void Send(string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = 5672,
                UserName = "wqxcpmsj",
                Password = "5ycB-owHLnQbJmWGDI1Iq7eAMcuOZile",
                VirtualHost = "wqxcpmsj"

            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            
            channel.ExchangeDeclare(
                "direct_exchange",
                "direct",
                false,
                false,
                null
            );
            
            var body = Encoding.UTF8.GetBytes(message); // set body

            channel.BasicPublish(exchange: "direct_exchange",
                routingKey: _queueName,
                basicProperties: null,
                body: body); // send message
        }
    }
}