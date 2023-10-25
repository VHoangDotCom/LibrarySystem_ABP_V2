using LibrarySystem.CoreDependencies.Paging;
using LibrarySystem.Managers.Books;
using LibrarySystem.Managers.Books.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibrarySystem.Services.Books
{
    public class BookAppService : LibrarySystemAppServiceBase, IBookAppService
    {
        private readonly BookManager _bookManager;

        public BookAppService(BookManager bookManager)
        {
            _bookManager = bookManager;
        }

        [HttpPost]
        public Task<GridResult<BookDto>> GetAllPaging(GridParam input)
        {
            return _bookManager.GetAllPaging(input);
        }

        [HttpGet]
        public Task<BookDto> GetById(long id)
        {
            return _bookManager.GetById(id);
        }

        [HttpPost]
        public Task<CreateBookDto> Create(CreateBookDto input)
        {
            return _bookManager.Create(input);
        }

        [HttpPut]
        public Task<UpdateBookDto> Update(UpdateBookDto input)
        {
            return _bookManager.Update(input);
        }

        [HttpDelete]
        public async Task Delete(long id)
        {
            await _bookManager.Delete(id);
        }
    }
}
