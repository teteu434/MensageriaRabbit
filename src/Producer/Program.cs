using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MensageriaRabbit.src.Producer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Environment.GetEnvironmentVariable("RABBIT_HOST") ?? "localhost";
            var user = Environment.GetEnvironmentVariable("RABBIT_USER") ?? "guest";
            var pass = Environment.GetEnvironmentVariable("RABBIT_PASS") ?? "guest";

            var factory = new ConnectionFactory()
            {
                HostName = host,
                UserName = user,
                Password = pass,
                Port = 5672
            };

            IConnection? connection = null;

            while (connection == null)
            {
                try
                {
                    Console.WriteLine("Tentando conectar ao RabbitMQ...");
                    connection = await factory.CreateConnectionAsync();
                }
                catch
                {
                    Console.WriteLine("Rabbit ainda não disponível, tentando de novo em 5s...");
                    await Task.Delay(5000);
                }
            }

            try
            {
                using var channel = await connection.CreateChannelAsync();
                await channel.ExchangeDeclareAsync(
                    exchange: "user-events",
                    type: ExchangeType.Fanout
                );

                Console.WriteLine("Producer iniciado!");

                while (true)
                {
                    try
                    {
                        Console.WriteLine("Digite uma mensagem para enviar:");
                        var message = Console.ReadLine();

                        if (message is null) continue;

                        var body = Encoding.UTF8.GetBytes(message);

                        var properties = new BasicProperties();

                        await channel.BasicPublishAsync(
                            exchange: "user-events",
                            routingKey: string.Empty,
                            mandatory: false,
                            basicProperties: properties,
                            body: body
                        );

                        Console.WriteLine("Mensagem enviada!");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Erro ao enviar mensagem: {ex.Message}");
                        await Task.Delay(5000);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro fatal: {ex.Message}");
                await Task.Delay(5000);
            }


        }
    }
}

