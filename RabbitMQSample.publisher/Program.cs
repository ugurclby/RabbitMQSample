using RabbitMQ.Client;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var factory = new ConnectionFactory(); /*{ HostName="localhost",Port= 5672 };*/ /*docker*/ 
        factory.Uri = new Uri("amqps://nkgocjdm:dmroGQVT_zmOFd2B86T3OK1lziG6Y33K@shark.rmq.cloudamqp.com/nkgocjdm"); 

        using (var connection = factory.CreateConnection())
        {
            var channel = connection.CreateModel();

            //channel.QueueDeclare("work-queue", true, false, false);

            channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

            Enumerable.Range(0, 50).ToList().ForEach(x =>
            {
                string message = $"{x} nolu log";
                var messageBody = Encoding.UTF8.GetBytes(message); 
                channel.BasicPublish("logs-fanout", string.Empty, null, messageBody); 
                Console.WriteLine("mesaj gönderildi.");
            }  
            ); 
            Console.ReadLine();
        }
    }
}