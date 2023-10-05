using RabbitMQ.Client;
using System.Text;

public enum LogEnums
{
    Critical = 0,
    Error = 1,
    Warning = 2,
    Info = 3
}
public class Program
{
    private static void Main(string[] args)
    {
        var factory = new ConnectionFactory(); /*{ HostName="localhost",Port= 5672 };*/ /*docker*/
        factory.Uri = new Uri("amqps://nkgocjdm:dmroGQVT_zmOFd2B86T3OK1lziG6Y33K@shark.rmq.cloudamqp.com/nkgocjdm");

        using (var connection = factory.CreateConnection())
        {
            var channel = connection.CreateModel();

            //channel.QueueDeclare("work-queue", true, false, false);

            //channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

            channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

            Enum.GetNames(typeof(LogEnums)).ToList().ForEach(
                e =>
                {
                    var routeKey = $"route-{e}";
                    var queuename = $"direct-queue-{e}";
                    channel.QueueDeclare(queuename, true, false, false);
                    channel.QueueBind(queuename, "logs-direct", routeKey,null);
                }
                ); 


            Enumerable.Range(0, 50).ToList().ForEach(x =>
            {
                LogEnums logEnum = (LogEnums)new Random().Next(0,4);
                var routeKey = $"route-{logEnum}";
                string message = $"{logEnum} {x} nolu log";
                var messageBody = Encoding.UTF8.GetBytes(message);
                //channel.BasicPublish("logs-fanout", string.Empty, null, messageBody);
                channel.BasicPublish("logs-direct",routeKey,null,messageBody);
                Console.WriteLine($"log gönderildi.{message}");
            }
            );
            Console.ReadLine();
        }
    }
}