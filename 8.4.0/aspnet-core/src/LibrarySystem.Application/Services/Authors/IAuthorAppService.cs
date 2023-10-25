using Abp.Application.Services;
using LibrarySystem.CoreDependencies.Paging;
using LibrarySystem.Managers.Authors.Dtos;
using System.Threading.Tasks;

namespace LibrarySystem.Services.Authors
{
    public interface IAuthorAppService : IApplicationService
    {
        Task<CreateAuthorDto> Create(CreateAuthorDto input);
        Task<GridResult<AuthorDto>> GetAllPaging(GridParam input);
        Task<UpdateAuthorDto> Update(UpdateAuthorDto input);
        Task Delete(long authorId);
        Task<AuthorDto> GetById(long authorId);
    }
}
