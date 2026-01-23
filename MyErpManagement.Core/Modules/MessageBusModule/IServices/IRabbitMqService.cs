using MyErpManagement.Core.Modules.MessageBusModule.Models;

namespace MyErpManagement.Core.Modules.MessageBusModule.IServices
{
    public interface IRabbitMqService {
        Task PublishEmailTaskAsync(EmailMessage message);
    }
}
