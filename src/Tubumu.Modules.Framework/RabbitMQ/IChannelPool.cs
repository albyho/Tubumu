using System;

namespace Tubumu.Modules.Framework.RabbitMQ
{
    public interface IChannelPool : IDisposable
    {
        IChannelAccessor Acquire(string channelName = null, string connectionName = null);
    }
}