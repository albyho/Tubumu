using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Tubumu.RabbitMQ
{
    /// <summary>
    /// ChannelPool
    /// </summary>
    public class ChannelPool : IChannelPool
    {
        private const string DefaultChannelName = "Default";
        private readonly IConnectionPool _connectionPool;
        private readonly ConcurrentDictionary<string, ChannelPoolItem> _channels;
        private readonly TimeSpan _totalDisposeWaitDuration;
        private readonly ILogger<ChannelPool> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionPool"></param>
        /// <param name="logger"></param>
        public ChannelPool(IConnectionPool connectionPool, ILogger<ChannelPool> logger)
        {
            _connectionPool = connectionPool;
            _channels = new ConcurrentDictionary<string, ChannelPoolItem>();
            _totalDisposeWaitDuration = TimeSpan.FromSeconds(10);
            _logger = logger;
        }

        /// <summary>
        /// Acquire
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public IChannelAccessor Acquire(string channelName = null, string connectionName = null)
        {
            CheckDisposed();

            channelName = channelName ?? DefaultChannelName;

            var poolItem = _channels.GetOrAdd(
                channelName,
                _ => new ChannelPoolItem(CreateChannel(connectionName))
            );

            poolItem.Acquire();

            return new ChannelAccessor(
                poolItem.Channel,
                channelName,
                () => poolItem.Release()
            );
        }

        private IModel CreateChannel(string connectionName)
        {
            return _connectionPool
                .Get(connectionName)
                .CreateModel();
        }

        private void CheckDisposed()
        {
            if (_disposedValue)
            {
                throw new ObjectDisposedException(nameof(ChannelPool));
            }
        }

        #region IDisposable Support

        private bool _disposedValue; // 要检测冗余调用

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
                return;

            if (disposing)
            {
                // 1: 释放托管状态(托管对象)。
                if (!_channels.Any())
                {
                    _logger.LogDebug($"Disposed channel pool with no channels in the pool.");
                    return;
                }

                var poolDisposeStopwatch = Stopwatch.StartNew();

                _logger.LogInformation($"Disposing channel pool ({_channels.Count} channels).");

                var remainingWaitDuration = _totalDisposeWaitDuration;

                foreach (var poolItem in _channels.Values)
                {
                    var poolItemDisposeStopwatch = Stopwatch.StartNew();

                    try
                    {
                        poolItem.WaitIfInUse(remainingWaitDuration);
                        poolItem.Dispose();
                    }
                    catch
                    {
                        // ignored
                    }

                    poolItemDisposeStopwatch.Stop();

                    remainingWaitDuration = remainingWaitDuration > poolItemDisposeStopwatch.Elapsed
                        ? remainingWaitDuration.Subtract(poolItemDisposeStopwatch.Elapsed)
                        : TimeSpan.Zero;
                }

                poolDisposeStopwatch.Stop();

                _logger.LogInformation($"Disposed RabbitMQ Channel Pool ({_channels.Count} channels in {poolDisposeStopwatch.Elapsed.TotalMilliseconds:0.00} ms).");

                if (poolDisposeStopwatch.Elapsed.TotalSeconds > 5.0)
                {
                    _logger.LogWarning($"Disposing RabbitMQ Channel Pool got time greather than expected: {poolDisposeStopwatch.Elapsed.TotalMilliseconds:0.00} ms.");
                }

                _channels.Clear();
            }

            // 2: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
            // 3: 将大型字段设置为 null。

            _disposedValue = true;
        }

        // 4: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~ChannelPool() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        /// <summary>
        /// Dispose
        /// </summary>
        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // 5: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
