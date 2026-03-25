using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Acme.BookStore.Configurations.Dtos
{
    public class CreateOrUpdateConfigurationInput
    {
        public string Key { get; set; }
        public JsonElement Value { get; set; }
    }
}
