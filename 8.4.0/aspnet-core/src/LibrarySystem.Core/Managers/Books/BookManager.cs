using Abp.Application.Services;
using Abp.UI;
using LibrarySystem.CoreDependencies.Extension;
using LibrarySystem.CoreDependencies.IOC;
using LibrarySystem.CoreDependencies.Paging;
using LibrarySystem.Entities;
using LibrarySystem.Managers.Authors.Dtos;
using LibrarySystem.Managers.Books.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LibrarySystem.Managers.Books
{
    public class BookManager : ApplicationService
    {
        private readonly IWorkScope _workScope;

        public BookManager(IWorkScope workScope)
        {
            _workScope = workScope;
        }

        public async Task<GridResult<BookDto>> GetAllPaging(GridParam input)
        {
            var query = _workScope.GetAll<Book>()
                        .Include(x => x.Author)
                        .Select(c => new BookDto
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Type = c.Type,
                            PublishDate = c.PublishDate,
                            Price = c.Price,
                            Author = new AuthorDto
                            {
                                Id = c.AuthorId,
                                Name = c.Author.Name,
                                BirthDate = c.Author.BirthDate,
                                ShortBio = c.Author.ShortBio,
                            }
                        });

            return await query.GetGridResult(query, input);
        }

        public async Task<BookDto> GetById(long id)
        {
            var existedBook = await _workScope.GetAll<Book>()
                                    .Include(x => x.Author)
                                    .Where(book => book.Id == id)
                                    .FirstOrDefaultAsync();
            if (existedBook == null)
                throw new UserFriendlyException($"Book with Id {id} is not existed!");

            var result = new BookDto
            {
                Id = existedBook.Id,
                Name = existedBook.Name,
                Type = existedBook.Type,
                PublishDate = existedBook.PublishDate,
                Price = existedBook.Price,
                Author = new AuthorDto
                {
                    Id = existedBook.AuthorId,
                    Name = existedBook.Author.Name,
                    BirthDate = existedBook.Author.BirthDate,
                    ShortBio = existedBook.Author.ShortBio,
                }
            };

            return result; 
        }

        public async Task<CreateBookDto> Create(CreateBookDto input)
        {
            var isDuplicated = await _workScope.GetAll<Book>().AnyAsync(x => x.Name == input.Name);

            if(isDuplicated)
                throw new UserFriendlyException(String.Format("Name already exist !"));

            var isFolderExisted = await _workScope.GetAll<Author>().AnyAsync(x => x.Id == input.AuthorId);
            if(!isFolderExisted)
                throw new UserFriendlyException(String.Format($"Author {input.AuthorId} does not exist !"));

            await _workScope.InsertAndGetIdAsync<Book>(ObjectMapper.Map<Book>(input));
            return input;
        }

        public async Task<UpdateBookDto> Update(UpdateBookDto input)
        {
            var updateBook = await _workScope.GetAll<Book>()
                            .Where(x => x.Id  == input.Id)
                            .FirstOrDefaultAsync();

            var isDuplicated = await _workScope.GetAll<Book>()
                            .AnyAsync(x => x.Id != input.Id && x.Name == input.Name);
            if(isDuplicated)
                throw new UserFriendlyException(String.Format("Name already exist !"));

            await _workScope.UpdateAsync(ObjectMapper.Map<UpdateBookDto, Book>(input, updateBook));

            return input;
        }

        public async Task Delete(long id)
        {
            var isExisted = await _workScope.GetAll<Book>().AnyAsync(x => x.Id == id);
            if (!isExisted)
                throw new UserFriendlyException($"Book with Id {id} is not existed!");

            await _workScope.DeleteAsync<Book>(id);
        }
    }
}
