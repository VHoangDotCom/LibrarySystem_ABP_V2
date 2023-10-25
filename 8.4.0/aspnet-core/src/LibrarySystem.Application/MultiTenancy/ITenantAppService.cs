using Abp.Application.Services;
using LibrarySystem.MultiTenancy.Dto;

namespace LibrarySystem.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

