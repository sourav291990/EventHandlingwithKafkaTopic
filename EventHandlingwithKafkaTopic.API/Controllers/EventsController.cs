using EventHandlingwithKafkaTopic.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EventHandlingwithKafkaTopic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IKafkaProducerService _eventProducer;
        private const string _topicName = "my-topic";
        public EventsController(IKafkaProducerService eventProducer)
        {
            _eventProducer = eventProducer;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] string message)
        {
            await _eventProducer.ProduceMessageAsync(_topicName, message);

            return Ok();
        }
    }
}
