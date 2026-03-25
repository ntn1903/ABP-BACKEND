using Acme.BookStore.Configurations.Dtos;
using Acme.BookStore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Configurations
{
    public interface IConfigureAppService : IApplicationService
    {
        Task<PagedResultDto<ConfigurationDto>> GetListAsync(GetConfigurationInput input);
        Task<ConfigurationDto> GetAsync(string id);
        Task UpdateAsync(CreateOrUpdateConfigurationInput input);
        Task RemoveCacheAsync(string key);
    }
}
