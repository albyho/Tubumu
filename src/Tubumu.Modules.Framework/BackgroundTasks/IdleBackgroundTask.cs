using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OrchardCore.BackgroundTasks;

namespace Tubumu.Modules.Framework.BackgroundTasks
{
    /// <summary>
    /// IdleBackgroundTask
    /// </summary>
    [BackgroundTask(Schedule = "* * * * *", Description = "Idle background task.")]
    public class IdleBackgroundTask : IBackgroundTask
    {
        private readonly ILogger<IdleBackgroundTask> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public IdleBackgroundTask(ILogger<IdleBackgroundTask> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// DoWorkAsync
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"IdleBackgroundTask: {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
