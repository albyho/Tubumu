using System;
using RabbitMQ.Client;

namespace Tubumu.RabbitMQ
{
    public interface IConnectionPool : IDisposable
    {
        IConnection Get(string connectionName = null);
    }
}
