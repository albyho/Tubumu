using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Tubumu.Modules.Framework.RabbitMQ
{
    public static class ModelExtensions
    {
        public static void BasicPublishJson(this IModel model, string exchange, string routingKey, bool mandatory, IBasicProperties basicProperties, object body)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
            var bodyJson = JsonConvert.SerializeObject(body, Formatting.None, serializerSettings);
            var bodyBytes = Encoding.UTF8.GetBytes(bodyJson);
            model.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                mandatory: mandatory,
                basicProperties: basicProperties,
                body: bodyBytes
            );
        }
    }
}
