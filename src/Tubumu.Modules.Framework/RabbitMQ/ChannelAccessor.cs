using System;
using RabbitMQ.Client;

namespace Tubumu.Modules.Framework.RabbitMQ
{
    internal class ChannelAccessor : IChannelAccessor
    {
        public IModel Channel { get; }

        public string Name { get; }

        private readonly Action _disposeAction;

        public ChannelAccessor(IModel channel, string name, Action disposeAction)
        {
            _disposeAction = disposeAction;
            Name = name;
            Channel = channel;
        }

        public void Dispose()
        {
            _disposeAction.Invoke();
        }
    }
}
