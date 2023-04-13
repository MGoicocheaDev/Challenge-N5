using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace web_api_lib_application.Logic.KafkaEvent
{
    internal static class KafkaProducer
    { 
        internal async static Task SendMessage(IProducer<Null, string> producer, IConfiguration _config, string operation)
        {
            var topic = _config.GetSection("TopicName").Value;
            var kafkaMessage = JsonConvert.SerializeObject(new { Id = Guid.NewGuid(), Operation = operation });
            //using (var producer = producerBuilder.Build())
            //{
            DeliveryReport<Null, string> produced = null;
                producer.Produce(topic, new Message<Null, string> { Value = kafkaMessage }, (m) => produced = m);
                producer.Flush(TimeSpan.FromSeconds(10));

            //}
        }
    }
}
