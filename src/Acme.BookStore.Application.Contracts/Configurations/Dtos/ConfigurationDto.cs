using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Configurations.Dtos
{
    public class ConfigurationDto : FullAuditedEntityDto<Guid>
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsEnable { get; set; }
    }
}
