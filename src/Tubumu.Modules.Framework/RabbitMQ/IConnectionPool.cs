using System;
using RabbitMQ.Client;

namespace Tubumu.Modules.Framework.RabbitMQ
{
    public interface IConnectionPool : IDisposable
    {
        IConnection Get(string connectionName = null);
    }
}
