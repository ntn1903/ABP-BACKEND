using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.BookStore.BackgroundJobs.LogApiJobs
{
    public class LogApiJobArgs
    {
        public string Url { get; set; }
        public string Method { get; set; }
    }
}
