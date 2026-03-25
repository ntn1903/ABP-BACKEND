using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Configurations.Dtos
{
    public class GetConfigurationInput : PagedAndSortedResultRequestDto
    {
        public string Key { get; set; }
    }
}
