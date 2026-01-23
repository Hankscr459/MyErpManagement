using MyErpManagement.Core.Modules.EmailModule.IServices;
using MyErpManagement.Core.Modules.MessageBusModule.constants;
using MyErpManagement.Core.Modules.MessageBusModule.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MyErpManagement.Api.BackgroundServices
{
    public class EmailConsumerWorker : BackgroundService
    {
        private readonly IConnectionFactory _factory;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EmailConsumerWorker> _logger;
        private readonly int _maxRetryCount;
        public EmailConsumerWorker(
            IConfiguration config,
            IServiceScopeFactory scopeFactory,
            ILogger<EmailConsumerWorker> logger)
        {
            _factory = new ConnectionFactory
            {
                HostName = config["Rabbit_MQ_Host"] ?? "",
                Port = int.Parse(config["Rabbit_MQ_Port"] ?? ""),
                UserName = config["Rabbit_MQ_UserName"] ?? "",
                Password = config["Rabbit_MQ_Password"] ?? ""
            };
            _scopeFactory = scopeFactory;
            _logger = logger;
            _maxRetryCount = config.GetValue<int>("Rabbit_MQ_Regist_Email_Max_Retry_Count", 3);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 建立連線
            using var connection = await _factory.CreateConnectionAsync(stoppingToken);
            using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            // 設定 QoS (Quality of Service) 規定一次拿一封信
            // 防止消費者一次塞進太多訊息
            // 設定 Prefetch Count (一次只取一條，處理完再拿下一條，避免負載不均)
            //
            // prefetchSize：訊息內容的大小限制（以位元組 Byte 為單位）。
            // 設定為 0：代表「無限制」。目前大多數的 RabbitMQ 客戶端與情境下，
            // 這個參數通常都設為 0，因為限制流量通常是看「訊息筆數」而非「位元組大小」。
            //
            // prefetchCount: 它定義了消費者在收到確認（Ack）之前，最多可以同時擁有的「未確認訊息」數量。
            // 設定為 1：代表「一次只拿一條訊息」。
            // 設定為 1目的實現公平分發（Fair Dispatch）。如果有的郵件發送很快，有的很慢，
            // RabbitMQ 會把新訊息交給已經閒下來的 Worker，而不是盲目地平均分配。
            await channel.BasicQosAsync(0, 1, false, stoppingToken);

            // 宣告隊列 (確保與生產者一致)
            await channel.QueueDeclareAsync(queue: QueueConstant.EmailRegistration, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            // ReceivedAsync寫好信件處理說明書
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<EmailMessage>(json);
                try
                {
                    if (message is not null)
                    {
                    
                        // 建立 Scope 以取得 Scoped 服務 (EmailService)
                        using var scope = _scopeFactory.CreateScope();
                        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                        _logger.LogInformation($"正在發送郵件至: {message.Email}");

                        // 呼叫原本的 EmailService 寄信
                        bool isSent = await emailService.SendRegisterCode(message.Email, message.VerificationCode);

                        if (isSent)
                        {
                            // 確認成功，將訊息從隊列移除
                            await channel.BasicAckAsync(ea.DeliveryTag, false);
                        }
                        else
                        {
                            throw new Exception("EmailService 回傳發送失敗");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "郵件寄送背景任務發生異常");
                    // 這裡可以選擇不 Ack，讓訊息重回隊列或丟入死信隊列 (DLQ)
                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
            };

            // 正式按下啟動鈕，開始從輸送帶接信
            // autoAck: 這是確保訊息**「不丟失」**的核心設定
            // false (手動確認)： RabbitMQ 發送訊息後，會將該訊息標記為「待確認（Unacked）」，但不會刪除它。
            // true (自動確認)： RabbitMQ 只要把訊息發送出去，就會立刻從記憶體 / 硬碟中刪除該訊息。
            await channel.BasicConsumeAsync(QueueConstant.EmailRegistration, autoAck: false, consumer: consumer);

            try
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Email 消費者服務正在停止...");
            }
        }
    }
}
