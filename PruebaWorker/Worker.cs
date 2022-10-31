using NATS.Client;
using System.Text;
using System;

namespace PruebaWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //GENERAR CODIGO PARA EMISOR Y RECEPTOR
            //EMISOR
            var cf = new ConnectionFactory();
            var c = cf.CreateConnection("nats://appnats:4222");
            var index = 1;

            while (!stoppingToken.IsCancellationRequested)
            {
                /*
                _logger.LogInformation("Worker 1 corriendo at: {time}", DateTimeOffset.Now);
                await Task.Delay(5000, stoppingToken);
                */
                // info-mensaje-evento
                var message = $"{index++} : Mensaje desde el automata!";
                Console.WriteLine($"{DateTime.Now:F} - Send: {message}");

                //publicación
                c.Publish("AUTOMATA", Encoding.UTF8.GetBytes(message));

                var message2 = $"{index++} : Mensaje desde el LPR!";
                Console.WriteLine($"{DateTime.Now:F} - Send: {message2}");
                c.Publish("LPR", Encoding.UTF8.GetBytes(message2));

                Thread.Sleep(1000);
            }
        }
    }
}