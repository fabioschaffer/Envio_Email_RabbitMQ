using RabbitMQ.Client;
using System.Text;

namespace CadastroPessoa.Services;

public class CriaMensagemRabbitMQ {
    public async Task CriaMensagem() {
        var factory = new ConnectionFactory() {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "fila_teste",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: new Dictionary<string, object?>
{
            { "email", "a@a.com" }
        }
        );

        var mensagem = "Mensagem teste";
        var body = Encoding.UTF8.GetBytes(mensagem);

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: "fila_teste",
            body: body
        );

        Console.WriteLine("Mensagem enviada!");
    }
}