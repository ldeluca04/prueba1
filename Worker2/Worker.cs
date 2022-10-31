using NATS.Client;

namespace Worker2
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
            //EJEMPLO DE CONSUMIDOR
            var cf = new ConnectionFactory();
            var c = cf.CreateConnection("nats://appnats:4222");//poner el puerto que corresponda

            string sTopico = "AUTOMATA";
            string sTopico2 = "LPR";

            //Suscripcion -> Evento que se dispara asincronamente cuando se recibe un mensaje
            EventHandler<MsgHandlerEventArgs> h = (sender, args) =>
            {
                Console.WriteLine($"{DateTime.Now:F} - Received: {args.Message}");
            };

            //Suscripción al topico            
            var sAsync = c.SubscribeAsync(sTopico);
            sAsync.MessageHandler += h; 
            sAsync.Start();

            var sAsync2 = c.SubscribeAsync(sTopico2);

            sAsync2.MessageHandler += h;
            sAsync2.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                //codigo de prueba... en este caso irá el que consume los mensajes del template que produce
                //_logger.LogInformation("Worker 2 corriendo at: {time}", DateTimeOffset.Now);
                //await Task.Delay(8000, stoppingToken);
            }

            //Console.ReadLine();
            // Desconectando
            sAsync.Unsubscribe();
            sAsync2.Unsubscribe();
            c.Drain(); c.Close();

        }
    }
}