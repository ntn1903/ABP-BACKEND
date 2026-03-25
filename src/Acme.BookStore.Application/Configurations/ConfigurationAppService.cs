using Acme.BookStore.Configurations.Dtos;
using Acme.BookStore.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Configurations
{
    public class ConfigurationAppService : ApplicationService, IConfigureAppService
    {
        public virtual string EnvironmentName => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower()?.Trim();
        public IRepository<Configuration, Guid> _configurationRepository => LazyServiceProvider.LazyGetRequiredService<IRepository<Configuration, Guid>>();
        public IDistributedCache<ConfigurationCacheItem> _configurationCache => LazyServiceProvider.LazyGetRequiredService<IDistributedCache<ConfigurationCacheItem>>();

        public async Task<PagedResultDto<ConfigurationDto>> GetListAsync(GetConfigurationInput input)
        {
            var queryable = await _configurationRepository.GetQueryableAsync();

            var items = queryable.Select(ObjectMapper.Map<Configuration, ConfigurationDto>).ToList();
            var totalCnt = queryable.Count();
            return new PagedResultDto<ConfigurationDto>(totalCnt, items);
        }

        public async Task<ConfigurationDto> GetAsync([SwaggerParameter(Description = "Id is key")] string id)
        {
            var data = await _configurationCache.GetOrAddAsync(GenerateCacheKey(id),
                async () =>
                {
                    var queryable = await _configurationRepository.GetQueryableAsync();
                    var entity = queryable.Where(m => m.Key == id).SingleOrDefault();

                    if (entity == null)
                    {
                        var insert = new Configuration { Key = id, IsEnable = true };
                        entity = await _configurationRepository.InsertAsync(insert, true);
                    }

                    var data = ObjectMapper.Map<Configuration, ConfigurationDto>(entity);
                    return new ConfigurationCacheItem { Item = data };
                },
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddHours(12)
                });

            return data.Item;
        }

        public async Task UpdateAsync(CreateOrUpdateConfigurationInput input)
        {
            var entity = await _configurationRepository.SingleOrDefaultAsync(m => m.Key == input.Key);
            var value = input.Value.ValueKind != JsonValueKind.Null && input.Value.ValueKind != JsonValueKind.Undefined ? input.Value.GetRawText() : null;

            if (entity == null)
            {
                var newEntity = new Configuration { Key = input.Key, Value = value };
                await _configurationRepository.InsertAsync(newEntity, true);
            }
            else
            {
                entity.Value = value;
                await _configurationRepository.UpdateAsync(entity, true);
            }

            await RemoveCacheAsync(input.Key);
        }

        protected virtual string GenerateCacheKey(string key)
        {
            return $"BookStore.{EnvironmentName}.Configurations.{key}";
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _configurationCache.RemoveAsync(GenerateCacheKey(key));
        }
    }
}
