using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MensageriaRabbit.src.Consumer1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
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

                using var channel = await connection.CreateChannelAsync();

                await channel.ExchangeDeclareAsync(
                    exchange: "user-events",
                    type: ExchangeType.Fanout
                );

                // fila exclusiva
                var queueResult = await channel.QueueDeclareAsync(
                    queue: "",
                    durable: false,
                    exclusive: true,
                    autoDelete: true
                );

                var queueName = queueResult.QueueName;

                // bind
                await channel.QueueBindAsync(
                    queue: queueName,
                    exchange: "user-events",
                    routingKey: ""
                );

                var consumer = new AsyncEventingBasicConsumer(channel);

                consumer.ReceivedAsync += async (sender, e) =>
                {
                    try
                    {
                        var msg = Encoding.UTF8.GetString(e.Body.ToArray());
                        Console.WriteLine($"[Consumer 1] Recebido: {msg}");
                        await channel.BasicAckAsync(e.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
                        await Task.Delay(5000);
                    }
                };

                await channel.BasicConsumeAsync(queueName, autoAck: false, consumer);

                Console.WriteLine("Consumer 1 rodando... Pressione Ctrl+C para sair.");

                var waitHandle = new ManualResetEventSlim(false);
                Console.CancelKeyPress += (sender, e) =>
                {
                    Console.WriteLine("Encerrando Consumer 3...");
                    waitHandle.Set();
                };

                waitHandle.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro fatal: {ex.Message}");
                await Task.Delay(5000);
            }

        }
    }
}

