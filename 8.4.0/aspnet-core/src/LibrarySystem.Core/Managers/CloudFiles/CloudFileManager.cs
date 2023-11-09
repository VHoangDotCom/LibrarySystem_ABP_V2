using Abp.Application.Services;
using Abp.UI;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LibrarySystem.CoreDependencies.Extension;
using LibrarySystem.CoreDependencies.Helper;
using LibrarySystem.CoreDependencies.IOC;
using LibrarySystem.CoreDependencies.Paging;
using LibrarySystem.Entities;
using LibrarySystem.Managers.CloudFiles.Dtos;
using LibrarySystem.Managers.CloudFolders.Dtos;
using LibrarySystem.Managers.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ResourceType = CloudinaryDotNet.Actions.ResourceType;

namespace LibrarySystem.Managers.CloudFiles
{
    public class CloudFileManager : ApplicationService
    {
        private readonly IWorkScope _workScope;
        private readonly CommonManager _commonManager;
        private readonly Cloudinary _cloudinary;

        public const string CLOUD_NAME = "dduv8pom4";
        public const string API_KEY = "952444439587681";
        public const string API_SECRET = "ubB0ir_v5YXR4KxmnZnuQHORoew";
        public CloudFileManager(
            IWorkScope workScope,
            CommonManager commonManager
            )
        {
            _workScope = workScope;
            _commonManager = commonManager;

            Account account = new Account(CLOUD_NAME, API_KEY, API_SECRET);
            _cloudinary = new Cloudinary(account);
        }

        #region LocalDB CloudFile

        public async Task<GridResult<CloudFileDto>> GetAllPaging(GridParam input)
        {
            var query = _workScope.GetAll<CloudFile>()
                .Include(x => x.CloudFolder)
                .Select(c => new CloudFileDto
                {
                    Id = c.Id,
                    PublicId = c.PublicId,
                    FileType = c.FileType,
                    ImageURL = c.ImageURL,
                    FileBase64 = c.FileBase64,
                    IsOverride = c.IsOverride,
                    FolderPath = c.FolderPath,
                    FolderId = c.FolderId,
                    CloudFolder = new CloudFolderDto
                    {
                        Id = c.CloudFolder.Id,
                        Name = c.CloudFolder.Name,
                        IsActive = c.CloudFolder.IsActive,
                        Level = c.CloudFolder.Level,
                        IsLeaf = c.CloudFolder.IsLeaf,
                        ParentId = c.CloudFolder.ParentId,
                        Code = c.CloudFolder.Code,
                        CombineName = c.CloudFolder.CombineName,
                    }
                });

            return await query.GetGridResult(query, input);
        }

        #endregion LocalDB CloudFile

        #region LocalDB-Cloud CloudFile

        public async Task<CloudFileDto> GetFileById(long fileId)
        {
            var existedFile = await _workScope.GetAll<CloudFile>()
                .Where(x => x.Id == fileId)
                .Select(x => new CloudFileDto
                {
                    Id = x.Id,
                    PublicId = x.PublicId,
                    FileType = x.FileType,
                    ImageURL = x.ImageURL,
                    FileBase64 = x.FileBase64,
                    IsOverride = x.IsOverride,
                    FolderPath = x.FolderPath,
                    FolderId = x.FolderId,
                    CloudFolder = new CloudFolderDto
                    {
                        Id = x.CloudFolder.Id,
                        Name = x.CloudFolder.Name,
                        IsActive = x.CloudFolder.IsActive,
                        Level = x.CloudFolder.Level,
                        IsLeaf = x.CloudFolder.IsLeaf,
                        ParentId = x.CloudFolder.ParentId,
                        Code = x.CloudFolder.Code,
                        CombineName = x.CloudFolder.CombineName,
                    }
                })
                .FirstOrDefaultAsync();

            if (existedFile == null)
                throw new UserFriendlyException($"File with Id '{fileId}' does not exist!");

            var isCloudExisted = CheckFileExistedInFolder(existedFile.PublicId, existedFile.FolderPath);
            if(!isCloudExisted)
                throw new UserFriendlyException($"File '{existedFile.PublicId}' exists in DB but it does not existed in Cloudinary!");

            return existedFile;
        }

        public async Task<CloudFileDto> CreateAndUploadFile(CreateCloudFileDto input)
        {
            // PNG = 3
            var isExisted = await _workScope.GetAll<CloudFile>()
                .AnyAsync(x => (x.PublicId.Trim().ToUpper() == input.PublicId.TrimEnd().ToUpper()) && x.FolderId == input.FolderId);
            if(isExisted)
                throw new UserFriendlyException(String.Format($"File with PublicID '{input.PublicId}' already exists in Folder '{input.FolderId}'"));

            var folderExisted = await _workScope.GetAll<CloudFolder>()
                .Where(x => x.Id == input.FolderId)
                .FirstOrDefaultAsync();
            if (folderExisted == null)
                throw new UserFriendlyException(String.Format($"Folder '{input.FolderId}' does not exists"));

            var fileMap = ObjectMapper.Map<CloudFile>(input);
            fileMap.FolderPath = folderExisted.CombineName;

            var uploadCloudinaryFile = new CreateCloudinaryFileDto()
            {
                PublicId = input.PublicId,
                FileType = input.FileType,
                FileBase64 = input.FileBase64,
                IsOverride = input.IsOverride,
                FolderName = fileMap.FolderPath
            };
            var cloudFileURL = await UploadFileCloudinary(uploadCloudinaryFile);
            if (cloudFileURL != null)
                fileMap.ImageURL = cloudFileURL;

            await _workScope.InsertAsync<CloudFile>(fileMap);

            var fileDto = new CloudFileDto()
            {
                Id = fileMap.Id,
                PublicId = fileMap.PublicId,
                FileType = fileMap.FileType,
                ImageURL = fileMap.ImageURL,
                FileBase64 = fileMap.FileBase64,
                IsOverride = fileMap.IsOverride,
                FolderPath = fileMap.FolderPath,
                FolderId = fileMap.FolderId,
                CloudFolder = new CloudFolderDto
                {
                    Id = fileMap.FolderId,
                    Name = fileMap.CloudFolder.Name,
                    IsActive = fileMap.CloudFolder.IsActive,
                    Level = fileMap.CloudFolder.Level,
                    IsLeaf = fileMap.CloudFolder.IsLeaf,
                    ParentId = fileMap.CloudFolder.ParentId,
                    Code = fileMap.CloudFolder.Code,
                    CombineName = fileMap.CloudFolder.CombineName,
                }
            };

            return fileDto;
        }

        public async Task<UpdatedCloudFileDto> UpdateFile(UpdateCloudFileDto input)
        {
            var existingFile = await _workScope.GetAll<CloudFile>()
                .Where(x => x.Id == input.Id)
                .FirstOrDefaultAsync();
            if(existingFile == null)
                throw new UserFriendlyException(String.Format($"File '{input.Id}' does not exists"));

            //Update File in Cloudinary
            var oldCloudinaryPublicId = existingFile.FolderPath + "/" + existingFile.PublicId;

            var updateCloudinaryFile = new UpdateCloudinaryFileDto()
            {
                PublicId = oldCloudinaryPublicId,
                FileType = input.FileType,
                FileBase64 = input.FileBase64,
                IsOverride = input.IsOverride
            };

            var cloudFileURL = await UpdateFileCloudinary(updateCloudinaryFile);
            if (cloudFileURL != null)
                existingFile.ImageURL = cloudFileURL;

            //Update File in Local DB
            existingFile.FileType = input.FileType;
            existingFile.FileBase64 = input.FileBase64;
            existingFile.IsOverride = input.IsOverride;

            await _workScope.UpdateAsync<CloudFile>(existingFile);
            CurrentUnitOfWork.SaveChanges();

            var fileDto = new UpdatedCloudFileDto()
            {
                Id = existingFile.Id,
                PublicId = existingFile.PublicId,
                FileType = existingFile.FileType,
                ImageURL = existingFile.ImageURL,
                FileBase64 = existingFile.FileBase64,
                IsOverride = existingFile.IsOverride,
                FolderPath = existingFile.FolderPath,
                FolderId = existingFile.FolderId
            };

            return fileDto;
        }

        public async Task<bool> DeleteFile(long id)
        {
            var existedFile = await _workScope.GetAll<CloudFile>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (existedFile == null)
                throw new UserFriendlyException($"File with ID '{id}' not found!");

            var cloudPublicId = existedFile.FolderPath + "/" + existedFile.PublicId;
            var isDeleteCloudinaryFile = await DeleteFileCloudinary(cloudPublicId);
            if (isDeleteCloudinaryFile)
            {
                await _workScope.DeleteAsync<CloudFile>(id);
                return true;
            }else
                return false;
        }

        #endregion LocalDB-Cloud CloudFile

        #region Cloud CloudFile

        private async Task<string> UploadFileCloudinary(CreateCloudinaryFileDto input)
        {
            var cloudinaryFormat = CloudinaryFormatMapper.MapToCloudinaryFormat(input.FileType);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(input.FileBase64),
                PublicId = input.PublicId,
                Overwrite = input.IsOverride,
                Faces = true,
                Folder = input.FolderName,
                Format = cloudinaryFormat
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == HttpStatusCode.OK || uploadResult.StatusCode == HttpStatusCode.Created)
                return uploadResult.Url.ToString();
            else
                throw new UserFriendlyException($"Cloudinary upload failed with status code: {uploadResult.StatusCode}");
        }

        private async Task<string> UpdateFileCloudinary(UpdateCloudinaryFileDto input)
        {
            var isExisted = CheckExistedFile(input.PublicId);
            if (!isExisted)
                throw new UserFriendlyException($"Cloudinary File with public ID '{input.PublicId}' not found.");

            var cloudinaryFormat = CloudinaryFormatMapper.MapToCloudinaryFormat(input.FileType);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(input.FileBase64),
                PublicId = input.PublicId,
                Overwrite = input.IsOverride,
                Faces = true,
                Format = cloudinaryFormat
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == HttpStatusCode.OK || uploadResult.StatusCode == HttpStatusCode.Created)
            {
                return uploadResult.Url.ToString();
            }
            else
            {
                throw new UserFriendlyException($"Cloudinary upload failed with status code: {uploadResult.StatusCode}");
            }
        }

        private async Task<bool> DeleteFileCloudinary(string publicId)
        {
            var existed = CheckExistedFile(publicId);
            if (existed == false)
                throw new UserFriendlyException($"Cloudinary File with public ID '{publicId}' not found.");

            DeletionParams deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image,
                Type = "upload"
            };

            DeletionResult deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            if (deletionResult.Result == "ok")
                return true;
            else
                return false;
        }

        private bool CheckExistedFile(string publicId)
        {
            if (publicId == null)
                throw new UserFriendlyException("Public ID cannot be null.");

            SearchResult result = _cloudinary.Search()
                .Expression($"public_id={publicId}")
                .Execute();

            if (result.Resources != null && result.Resources.Count > 0)
                return true;
            else
                return false;
        }

        private bool CheckFileExistedInFolder(string publicId, string folder)
        {
            string assetPublicId = $"{folder}/{publicId}";

            SearchResult result = _cloudinary.Search()
               .Expression($"folder={folder} AND public_id={assetPublicId}")
               .Execute();

            if (result.Resources != null && result.Resources.Count > 0)
                return true;
            else
                return false;
        }


        #endregion Cloud CloudFile
    }
}
