using Abp.Application.Services;
using Abp.UI;
using LibrarySystem.CoreDependencies.Extension;
using LibrarySystem.CoreDependencies.IOC;
using LibrarySystem.CoreDependencies.Paging;
using LibrarySystem.Entities;
using LibrarySystem.Managers.Authors.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LibrarySystem.Managers.Authors
{
    public class AuthorManager : ApplicationService
    {
        private readonly IWorkScope _workScope;

        public AuthorManager(IWorkScope workScope)
        {
            _workScope = workScope;
        }

        public async Task<GridResult<AuthorDto>> GetAllPaging(GridParam input)
        {
            var query = _workScope.GetAll<Author>()
                .Select(c => new AuthorDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    ShortBio = c.ShortBio,
                });

            return await query.GetGridResult(query, input);
        }

        public async Task<AuthorDto> GetById(long authorId)
        {
            var existedAuthor = await _workScope.GetAll<Author>()
                        .Where(x => x.Id == authorId)
                        .FirstOrDefaultAsync();

            if (existedAuthor == null)
                throw new UserFriendlyException($"Author with Id {authorId} is not existed!");
            
            var author = new AuthorDto
            {
                Id = existedAuthor.Id,
                Name = existedAuthor.Name,
                BirthDate = existedAuthor.BirthDate,
                ShortBio = existedAuthor.ShortBio,
            };

            return author;
        }

        public async Task<CreateAuthorDto> Create(CreateAuthorDto input)
        {
            var isExisted = await _workScope.GetAll<Author>().AnyAsync(x => x.Name == input.Name);

            if (isExisted)
                throw new UserFriendlyException(String.Format("Name already exist !"));

            await _workScope.InsertAndGetIdAsync<Author>(ObjectMapper.Map<Author>(input));
            return input;
        }

        public async Task<UpdateAuthorDto> Update(UpdateAuthorDto input)
        {
            var updateAuthor = await _workScope.GetAsync<Author>(input.Id);

            var isExisted = await _workScope.GetAll<Author>().AnyAsync(x => x.Id != input.Id && x.Name == input.Name);
            if (isExisted)
                throw new UserFriendlyException(String.Format("Name already exist !"));

            await _workScope.UpdateAsync(ObjectMapper.Map<UpdateAuthorDto, Author>(input, updateAuthor));
            return input;
        }

        public async Task Delete(long authorId)
        {
            var isExisted = await _workScope.GetAll<Author>().AnyAsync(x => x.Id == authorId);
            if (!isExisted)
                throw new UserFriendlyException($"Author with Id {authorId} is not existed!");

            await _workScope.DeleteAsync<Author>(authorId);
        }
    }
}
