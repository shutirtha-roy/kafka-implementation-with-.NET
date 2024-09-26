using Confluent.Kafka;
static void Main(string[] args)
{
    var config = new ConsumerConfig
    {
        BootstrapServers = "localhost:9092",
        GroupId = "test-consumer-group",
        AutoOffsetReset = AutoOffsetReset.Earliest
    };

    using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
    {
        consumer.Subscribe("test-topic");

        try
        {
            while (true)
            {
                var consumeResult = consumer.Consume();
                Console.WriteLine($"Received message: {consumeResult.Message.Value}");
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }
}