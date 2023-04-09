using Confluent.Kafka;

namespace web_api_lib_application.Logic.KafkaEvent
{
    internal static class KafkaProducer
    { 
        internal async static Task SendMessage(ProducerConfig configuration, string topicName, string message)
        {
            using (var producer = new ProducerBuilder<Null, string>(configuration).Build())
            {
                await producer.ProduceAsync(topicName, new Message<Null, string> { Value = message });
                producer.Flush(TimeSpan.FromSeconds(10));

            }
        }
    }
}
