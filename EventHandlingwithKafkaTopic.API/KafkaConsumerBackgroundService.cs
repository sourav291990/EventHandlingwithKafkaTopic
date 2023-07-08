using EventHandlingwithKafkaTopic.Services.Contracts;

namespace EventHandlingwithKafkaTopic.API
{
    public class KafkaConsumerBackgroundService : BackgroundService
    {
        private readonly ILogger<KafkaConsumerBackgroundService> _logger;
        private readonly IKafkaConsumerService _kafkaConsumer;

        public KafkaConsumerBackgroundService(IKafkaConsumerService kafkaConsumer, ILogger<KafkaConsumerBackgroundService> logger)
        {
            _logger = logger;
            _kafkaConsumer = kafkaConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _kafkaConsumer.StartConsumingAsync(stoppingToken);
        }
    }
}
