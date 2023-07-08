using Confluent.Kafka;
using EventHandlingwithKafkaTopic.Services.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHandlingwithKafkaTopic.Services.Implementation
{
    public class KafkaPublisherService : IKafkaProducerService, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<KafkaPublisherService> _logger;
        private readonly ProducerConfig _producerConfig;

        public KafkaPublisherService(string bootstrapServers, ILogger<KafkaPublisherService> logger)
        {
            _logger = logger;
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            _producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }

        public async Task ProduceMessageAsync(string topic, string message)
        {
            var messageKey = Guid.NewGuid().ToString(); // Generate a unique key for the message

            var deliveryResult = await _producer.ProduceAsync(topic, new Message<Null, string>
            {
                Key = null,
                Value = message,
                Headers = new Headers {
                new Header("event-key", Encoding.UTF8.GetBytes(messageKey))
                }
            });
            _logger.LogInformation($"Published event to topic '{deliveryResult.Topic}', partition {deliveryResult.Partition}, offset {deliveryResult.Offset}");
        }
    }
}
