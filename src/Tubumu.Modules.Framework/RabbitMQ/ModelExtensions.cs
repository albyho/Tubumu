using System.Text;
using RabbitMQ.Client;
using Tubumu.Core.Extensions.Object;

namespace Tubumu.Modules.Framework.RabbitMQ
{
    public static class ModelExtensions
    {
        public static void BasicPublishJson(this IModel model, string exchange, string routingKey, bool mandatory, IBasicProperties basicProperties, object body)
        {
            var bodyJson = body.ToJson();
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
