using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Workers.Rabbit;

public class RabbitConsumerWorker : BackgroundService {
    private const string QUEUE_NAME = "fila_1";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        var factory = new ConnectionFactory { HostName = "localhost" };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: QUEUE_NAME,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: new Dictionary<string, object?> {
                { "x-queue-type", "quorum" }
            });

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (model, ea) => {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());

            Console.WriteLine($"[x] Received: {message}");

            // simula processamento real
            await Task.Delay(500);

            await channel.BasicAckAsync(ea.DeliveryTag, false);
        };

        await channel.BasicConsumeAsync(
            queue: QUEUE_NAME,
            autoAck: false,
            consumer: consumer);

        Console.WriteLine("Consumer iniciado...");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}