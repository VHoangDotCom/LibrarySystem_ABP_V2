using Abp.Application.Services;
using LibrarySystem.CoreDependencies.Paging;
using LibrarySystem.Managers.Books.Dtos;
using System.Threading.Tasks;

namespace LibrarySystem.Services.Books
{
    public interface IBookAppService : IApplicationService
    {
        Task<GridResult<BookDto>> GetAllPaging(GridParam input);
        Task<BookDto> GetById(long id);
        Task<CreateBookDto> Create(CreateBookDto input);
        Task<UpdateBookDto> Update(UpdateBookDto input);
        Task Delete(long id);
    }
}
