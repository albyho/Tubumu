using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OrchardCore.BackgroundTasks;

namespace Tubumu.Modules.Framework.BackgroundTasks
{
    /// <summary>
    /// NewDayBackgroundTask 北京时间 00:00:00，调度器使用的是 UTC 时间，故减少8小时应该设置为 16:00:00
    /// </summary>
    [BackgroundTask(Schedule = "0 16 * * *", Description = "New day background task.")]
    public class NewDayBackgroundTask : IBackgroundTask
    {
        private readonly ILogger<NewDayBackgroundTask> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        public NewDayBackgroundTask(ILogger<NewDayBackgroundTask> logger)
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
            _logger.LogInformation($"NewDayBackgroundTask: {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
