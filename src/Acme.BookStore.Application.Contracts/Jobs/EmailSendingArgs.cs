using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.BookStore.Jobs
{
    //[BackgroundJobName("emails")]
    public class EmailSendingArgs
    {
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
