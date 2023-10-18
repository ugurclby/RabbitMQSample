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

            //channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

            //Enum.GetNames(typeof(LogEnums)).ToList().ForEach(
            //    e =>
            //    {
            //        var routeKey = $"route-{e}";
            //        var queuename = $"direct-queue-{e}";
            //        channel.QueueDeclare(queuename, true, false, false);
            //        channel.QueueBind(queuename, "logs-direct", routeKey, null);
            //    }
            //    );

            //channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);

            //Random rnd = new Random();

            //Enumerable.Range(0, 50).ToList().ForEach(x =>
            //{
            //    LogEnums logEnum1 = (LogEnums)rnd.Next(0, 4);
            //    LogEnums logEnum2 = (LogEnums)rnd.Next(0, 4);
            //    LogEnums logEnum3 = (LogEnums)rnd.Next(0, 4); 

            //    var routeKey = $"{logEnum1}.{logEnum2}.{logEnum3}";
            //    string message = $"{logEnum1}.{logEnum2}.{logEnum3} nolu log";
            //    var messageBody = Encoding.UTF8.GetBytes(message);
            //    //channel.BasicPublish("logs-fanout", string.Empty, null, messageBody);
            //    //channel.BasicPublish("logs-direct", routeKey, null, messageBody);
            //    channel.BasicPublish("logs-topic", routeKey, null, messageBody);
            //    Console.WriteLine($"log gönderildi.{message}");
            //}
            //);

            Dictionary<string,Object> headers = new Dictionary<string,Object>();

            headers.Add("format", "pdf");
            headers.Add("shape", "a4");

            var properties=channel.CreateBasicProperties();
            properties.Headers = headers;

            channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

            channel.BasicPublish("header-exchange", String.Empty, properties, Encoding.UTF8.GetBytes("Header Exchange Deneme"));

            Console.ReadLine();
        }
    }
}