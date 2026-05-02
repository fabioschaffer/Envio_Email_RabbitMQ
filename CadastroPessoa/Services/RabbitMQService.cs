using RabbitMQ.Client;
using System.Text;

namespace CadastroPessoa.Services;

public class CriaMensagemRabbitMQ {

    const string QUEUE_NAME = "fila_1";

    public async Task CriaMensagem() {

        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: QUEUE_NAME, durable: true, exclusive: false, autoDelete: false,
            arguments: new Dictionary<string, object?> { { "x-queue-type", "quorum" } });

        const string message = "Hello World!";
        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: QUEUE_NAME, body: body);

        Console.WriteLine($"Sent {message}");
    }
}