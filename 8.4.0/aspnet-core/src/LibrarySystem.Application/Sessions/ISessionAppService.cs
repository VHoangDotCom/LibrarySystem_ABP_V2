using System.Threading.Tasks;
using Abp.Application.Services;
using LibrarySystem.Sessions.Dto;

namespace LibrarySystem.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
