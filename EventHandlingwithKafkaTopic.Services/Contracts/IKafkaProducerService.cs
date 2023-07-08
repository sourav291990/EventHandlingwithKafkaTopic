using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHandlingwithKafkaTopic.Services.Contracts
{
    public interface IKafkaProducerService
    {
        void Dispose();
        Task ProduceMessageAsync(string topic, string message);
    }
}
