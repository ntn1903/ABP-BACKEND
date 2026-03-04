using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;

namespace Acme.BookStore.Jobs
{
    public class EmailSendingJob : AsyncBackgroundJob<EmailSendingArgs>, ITransientDependency
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<EmailSendingJob> _logger;

        public EmailSendingJob(IEmailSender emailSender, ILogger<EmailSendingJob> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public override async Task ExecuteAsync(EmailSendingArgs args)
        {
            await _emailSender.SendAsync(
                args.EmailAddress,
                args.Subject,
                args.Body
            );

            _logger.LogInformation("-----------------End------------------");
        }
    }
}
