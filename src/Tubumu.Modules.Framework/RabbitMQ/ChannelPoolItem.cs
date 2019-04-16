using System;
using System.Threading;
using RabbitMQ.Client;

namespace Tubumu.Modules.Framework.RabbitMQ
{
    internal class ChannelPoolItem : IDisposable
    {
        public IModel Channel { get; }

        public bool IsInUse
        {
            get => _isInUse;
            private set => _isInUse = value;
        }
        private volatile bool _isInUse;

        public ChannelPoolItem(IModel channel)
        {
            Channel = channel;
        }

        public void Acquire()
        {
            lock (this)
            {
                while (IsInUse)
                {
                    Monitor.Wait(this);
                }

                IsInUse = true;
            }
        }

        public void WaitIfInUse(TimeSpan timeout)
        {
            lock (this)
            {
                if (!IsInUse)
                {
                    return;
                }

                Monitor.Wait(this, timeout);
            }
        }

        public void Release()
        {
            lock (this)
            {
                IsInUse = false;
                Monitor.PulseAll(this);
            }
        }

        public void Dispose()
        {
            Channel.Dispose();
        }
    }
}
