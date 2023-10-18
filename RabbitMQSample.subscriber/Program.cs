using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Unicode;

internal class Program
{
    private static void Main(string[] args)
    {
        var factory = new ConnectionFactory(); /*{ HostName="localhost",Port= 5672 };*/ /*docker*/
        factory.Uri = new Uri("amqps://nkgocjdm:dmroGQVT_zmOFd2B86T3OK1lziG6Y33K@shark.rmq.cloudamqp.com/nkgocjdm");

        using (var connection = factory.CreateConnection())
        {
            var channel = connection.CreateModel(); // kanal oluştur

            //channel.QueueDeclare("test-queue", true, false, false);
            string queueName = channel.QueueDeclare().QueueName;

            /*channel.QueueBind(queueName, "logs-fanout","", null);*/ // fanout exchange ile bağla ve kuyruk bind et. Kapatınca kuyruğu siler.


            channel.BasicQos(0, 1, false); // her seferinde 1 tane mesaj alıp işleyecek.

            var consumer = new EventingBasicConsumer(channel); // mesajları dinleyecek olan consumer

            //var routeKey = "*.Error.*"; // Burayı farklı örneklendirebilirsin. Ortası Error olanları al
            ////var routeKey = "Info.#"; // Info ile başlayanları al

            //channel.QueueBind(queueName, "logs-topic", routeKey);

            Dictionary<string, Object> headers = new Dictionary<string, Object>();

            headers.Add("format", "pdf");
            headers.Add("shape", "a4");
            headers.Add("x-match", "all");

            channel.QueueBind(queueName, "header-exchange", String.Empty, headers);
            channel.BasicConsume(queueName, false, consumer); // mesajları dinlemeye başla

            // autoack true yaptığımda mesajları okudu ve sildi. Console yazmadı bile.

            Console.WriteLine("loglar dinleniyor."); 

            consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine("Gelen Log :" + message);
                //File.AppendAllText("log-information", message + "\n");

                channel.BasicAck(e.DeliveryTag, false);
            };

            Console.ReadLine();
        } 
    } 
}