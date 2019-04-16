using System;
using System.Collections.Concurrent;
using RabbitMQ.Client;

namespace Tubumu.Modules.Framework.RabbitMQ
{
    /// <summary>
    /// ConnectionPool
    /// </summary>
    public class ConnectionPool : IConnectionPool
    {
        private const string DefaultHostName = "localhost";
        private const string DefaultConnectionName = "Default";
        private readonly ConnectionFactory _connectionFactory;
        private readonly ConcurrentDictionary<string, IConnection> _connections;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConnectionPool()
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = DefaultHostName,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                HandshakeContinuationTimeout = TimeSpan.FromSeconds(60),
            };
            _connections = new ConcurrentDictionary<string, IConnection>();
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public virtual IConnection Get(string connectionName = null)
        {
            connectionName = connectionName
                             ?? DefaultConnectionName;

            return _connections.GetOrAdd(
                connectionName,
                (_) => _connectionFactory.CreateConnection());
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
                foreach (var connection in _connections.Values)
                {
                    try
                    {
                        connection.Dispose();
                    }
                    catch
                    {
                        // ignored
                    }
                }

                _connections.Clear();
            }

            // 2: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
            // 3: 将大型字段设置为 null。

            _disposedValue = true;
        }

        // 4: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~ConnectionPool() {
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
