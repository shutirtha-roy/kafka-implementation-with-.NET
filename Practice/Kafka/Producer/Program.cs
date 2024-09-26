using Confluent.Kafka;

static void Main(string[] args)
{
    var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

    using (var producer = new ProducerBuilder<Null, string>(config).Build())
    {
        while (true)
        {
            Console.Write("Enter a message (or 'exit' to quit): ");
            string input = Console.ReadLine();

            if (input.ToLower() == "exit")
                break;

            producer.Produce("test-topic", new Message<Null, string> { Value = input },
                (deliveryReport) =>
                {
                    if (deliveryReport.Error.Code != ErrorCode.NoError)
                    {
                        Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                    }
                    else
                    {
                        Console.WriteLine($"Produced message to: {deliveryReport.TopicPartitionOffset}");
                    }
                });

            producer.Flush(TimeSpan.FromSeconds(10));
        }
    }
}