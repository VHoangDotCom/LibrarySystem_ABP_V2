using LibrarySystem.CoreDependencies.Paging;
using LibrarySystem.Managers.CloudFiles;
using LibrarySystem.Managers.CloudFiles.Dtos;
using System.Threading.Tasks;
using LibrarySystem;
using LibrarySystem.Services.CloudFiles;
using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Entities;

namespace LibrarySystem.Services.CloudFiles
{
    public class CloudFileAppService : LibrarySystemAppServiceBase, ICloudFileAppService
    {
        private readonly CloudFileManager _cloudFileManager;
        public CloudFileAppService(CloudFileManager cloudFileManager)
        {
            _cloudFileManager = cloudFileManager;
        }

        [HttpPost]
        public Task<GridResult<CloudFileDto>> GetAllPaging(GridParam input)
        {
            return _cloudFileManager.GetAllPaging(input);
        }

        [HttpGet]
        public Task<CloudFileDto> GetFileById(long fileId)
        {
            return _cloudFileManager.GetFileById(fileId);
        }

        [HttpPost]
        public Task<CloudFileDto> CreateAndUploadFile(CreateCloudFileDto input)
        {
            return _cloudFileManager.CreateAndUploadFile(input);
        }

        [HttpPost]
        public Task<UpdatedCloudFileDto> UpdateFile(UpdateCloudFileDto input)
        {
            return _cloudFileManager.UpdateFile(input);
        }

        [HttpDelete]
        public Task<bool> DeleteFile(long id)
        {
            return _cloudFileManager.DeleteFile(id);
        }
    }
}
