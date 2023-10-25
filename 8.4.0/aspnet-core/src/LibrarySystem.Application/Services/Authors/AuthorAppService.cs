using LibrarySystem.CoreDependencies.Paging;
using LibrarySystem.Managers.Authors;
using LibrarySystem.Managers.Authors.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibrarySystem.Services.Authors
{
    public class AuthorAppService : LibrarySystemAppServiceBase, IAuthorAppService
    {
        private readonly AuthorManager _authorManager;
        public AuthorAppService(AuthorManager authorManager)
        {
            _authorManager = authorManager;
        }

        [HttpPost]
        public async Task<CreateAuthorDto> Create(CreateAuthorDto input)
        {
            return await _authorManager.Create(input);
        }

        [HttpPost]
        public async Task<GridResult<AuthorDto>> GetAllPaging(GridParam input)
        {
            return await _authorManager.GetAllPaging(input);
        }

        [HttpPut]
        public async Task<UpdateAuthorDto> Update(UpdateAuthorDto input)
        {
            return await _authorManager.Update(input);
        }

        [HttpDelete]
        public async Task Delete(long authorId)
        {
            await _authorManager.Delete(authorId);
        }

        [HttpGet]
        public Task<AuthorDto> GetById(long authorId)
        {
            return _authorManager.GetById(authorId);
        }
    }
}
