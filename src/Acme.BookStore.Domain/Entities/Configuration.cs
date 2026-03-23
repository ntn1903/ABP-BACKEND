using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Entities
{
    [Index(nameof(Key), IsUnique = true)]
    public class Configuration : FullAuditedEntity<Guid>
    {
        [Required]
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsEnable { get; set; }

        public T GetObjectValue<T>()
        {
            return JsonConvert.DeserializeObject<T>(Value);
        }
    }
}
