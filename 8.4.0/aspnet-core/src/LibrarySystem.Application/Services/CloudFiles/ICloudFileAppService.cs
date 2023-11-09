using Abp.Application.Services;
using LibrarySystem.CoreDependencies.Paging;
using LibrarySystem.Entities;
using LibrarySystem.Managers.CloudFiles.Dtos;
using System.Threading.Tasks;

namespace LibrarySystem.Services.CloudFiles
{
    public interface ICloudFileAppService : IApplicationService
    {
        Task<GridResult<CloudFileDto>> GetAllPaging(GridParam input);
        Task<CloudFileDto> GetFileById(long fileId);
        Task<CloudFileDto> CreateAndUploadFile(CreateCloudFileDto input);
        Task<UpdatedCloudFileDto> UpdateFile(UpdateCloudFileDto input);
        Task<bool> DeleteFile(long id);
    }
}
