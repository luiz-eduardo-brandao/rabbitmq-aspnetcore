using EasyNetQ;
using EasyNetQ.Console;
using Newtonsoft.Json;

const string EXCHANGE = "curso-rabbitmq";
const string QUEUE = "person-created";
const string ROUTING_KEY = "hr.person-created";

var person = new Person("Edu", "123", new DateTime(1999, 10, 22));

var bus = RabbitHutch.CreateBus("host=localhost");

var advanced = bus.Advanced;

var exchange = advanced.ExchangeDeclare(EXCHANGE, "topic");
var queue = advanced.QueueDeclare(QUEUE);

advanced.Publish(exchange, ROUTING_KEY, true, new Message<Person>(person));

advanced.Consume<Person>(queue, (msg, info) =>
{
    var json = JsonConvert.SerializeObject(msg.Body);
    Console.WriteLine(json);
});

await bus.PubSub.PublishAsync(person);

await bus.PubSub.SubscribeAsync<Person>("marketing", msg =>
{
   var json = JsonConvert.SerializeObject(msg);
   Console.WriteLine(json);
});

Console.ReadLine();
