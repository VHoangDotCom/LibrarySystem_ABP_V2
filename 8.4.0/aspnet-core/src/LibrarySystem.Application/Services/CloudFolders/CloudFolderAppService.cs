using LibrarySystem.Managers.CloudFolders;
using LibrarySystem.Managers.CloudFolders.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibrarySystem.Services.CloudFolders
{
    public class CloudFolderAppService : LibrarySystemAppServiceBase, ICloudFolderAppService
    {
        private readonly CloudFolderManager _cloudFolderManager;
        public CloudFolderAppService(CloudFolderManager cloudFolderManager)
        {
            _cloudFolderManager = cloudFolderManager;
        }

        [HttpPost]
        public Task<TreeFolderDto> GetAll(InputToGetFolderDto input)
        {
            return _cloudFolderManager.GetAll(input);
        }

        [HttpGet]
        public Task<CloudFolderDto> GetFolderById(long folderId)
        {
            return _cloudFolderManager.GetFolderById(folderId);
        }

        [HttpPost]
        public Task<CreateCloudFolderDto> CreateFolder(CreateCloudFolderDto input)
        {
            return _cloudFolderManager.CreateFolder(input);
        }

        [HttpPost]
        public Task<UpdateCloudFolderDto> UpdateFolder(UpdateCloudFolderDto input)
        {
            return _cloudFolderManager.UpdateFolder(input);
        }

        [HttpDelete]
        public Task DeleteFolder(long id)
        {
            return _cloudFolderManager.DeleteFolder(id);
        }

        [HttpDelete]
        public Task<bool> DeleteCloudFolder(string folderName)
        {
            return _cloudFolderManager.DeleteCloudinaryFolder(folderName);
        }
    }
}
