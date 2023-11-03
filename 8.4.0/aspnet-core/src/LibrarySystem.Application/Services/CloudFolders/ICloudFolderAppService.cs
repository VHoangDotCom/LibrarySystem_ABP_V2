using Abp.Application.Services;
using LibrarySystem.Managers.CloudFolders.Dtos;
using System.Threading.Tasks;

namespace LibrarySystem.Services.CloudFolders
{
    public interface ICloudFolderAppService : IApplicationService
    {
        Task<TreeFolderDto> GetAll(InputToGetFolderDto input);
        Task<CloudFolderDto> GetFolderById(long folderId);
        Task<CreateCloudFolderDto> CreateFolder(CreateCloudFolderDto input);
        Task<UpdateCloudFolderDto> UpdateFolder(UpdateCloudFolderDto input);
        Task DeleteFolder(long id);
    }
}
