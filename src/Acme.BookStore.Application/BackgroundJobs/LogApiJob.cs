using Acme.BookStore.BackgroundJobs.LogApiJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace Acme.BookStore.BackgroundJobs
{
    public class LogApiJob : AsyncBackgroundJob<LogApiJobArgs>, ITransientDependency
    {
        private readonly ILogger<LogApiJob> _logger;

        public LogApiJob(ILogger<LogApiJob> logger)
        {
            _logger = logger;
        }

        public override Task ExecuteAsync(LogApiJobArgs args)
        {
            //_logger.LogInformation($"Begin {nameof(LogApiJob)} --------------------------------------------");
            _logger.LogWarning($"[BackgroundJob] API Called: {args.Method} {args.Url}");
            return Task.CompletedTask;
        }
    }
}
