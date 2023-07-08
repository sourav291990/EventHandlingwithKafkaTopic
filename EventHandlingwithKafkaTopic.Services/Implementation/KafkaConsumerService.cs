using Confluent.Kafka;
using EventHandlingwithKafkaTopic.Services.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EventHandlingwithKafkaTopic.Services.Implementation
{
    public class KafkaConsumerService : IKafkaConsumerService, IDisposable
    {
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly string _bootstrapServers;
        private const string _topic = "my-topic";
        private readonly ConsumerConfig _consumerConfig;
        private const string _groupId = "my-consumer-group";
        private readonly IConsumer<Ignore, string> _consumer;

        public KafkaConsumerService(ILogger<KafkaConsumerService> logger, string bootstrapServers)
        {
            _logger = logger;
            _bootstrapServers = bootstrapServers;
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
            _consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(_topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(cancellationToken);
                    if (consumeResult != null)
                    {
                        await ProcessMessageAsync(consumeResult.Message);
                        _consumer.Commit(consumeResult);
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            finally
            {
                _consumer.Close();
            }
        }

        private async Task ProcessMessageAsync(Message<Ignore, string> message)
        {
            _logger.LogInformation($"Received message: {message.Value}");
            // Add your message processing logic here
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }
    }
}
