using System.Threading.Tasks;
using Abp.Application.Services;
using LibrarySystem.Authorization.Accounts.Dto;

namespace LibrarySystem.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
