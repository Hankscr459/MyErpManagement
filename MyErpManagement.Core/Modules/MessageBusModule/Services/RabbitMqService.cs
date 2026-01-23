using Microsoft.Extensions.Configuration;
using MyErpManagement.Core.Modules.MessageBusModule.constants;
using MyErpManagement.Core.Modules.MessageBusModule.IServices;
using MyErpManagement.Core.Modules.MessageBusModule.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MyErpManagement.Core.Modules.MessageBusModule.Services
{
    public class RabbitMqService(IConfiguration config) : IRabbitMqService
    {
        public async Task PublishEmailTaskAsync(EmailMessage message)
        {
            var factory = new ConnectionFactory
            {
                HostName = config["Rabbit_MQ_Host"] ?? "",
                Port = int.Parse(config["Rabbit_MQ_Port"] ?? ""),
                UserName = config["Rabbit_MQ_UserName"] ?? "",
                Password = config["Rabbit_MQ_Password"] ?? ""
            };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: QueueConstant.EmailRegistration, durable: true, exclusive: false, autoDelete: false);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            // 訊息持久化
            var properties = new BasicProperties { DeliveryMode = DeliveryModes.Persistent };

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: QueueConstant.EmailRegistration,
                mandatory: true,
                basicProperties: properties,
                body: body);
        }
    }
}
