using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LibrarySystem.Constants.Enum;
using LibrarySystem.Entities;
using LibrarySystem.Managers.CloudFolders.Dtos;

namespace LibrarySystem.Managers.CloudFiles.Dtos
{
    [AutoMapTo(typeof(CloudFile))]
    public class CloudFileDto : EntityDto<long>
    {
        public string PublicId { get; set; }
        public FileType FileType { get; set; }
        public string ImageURL { get; set; }
        public string FileBase64 { get; set; }
        public bool IsOverride { get; set; }
        public string FolderPath { get; set; }
        public long FolderId { get; set; }
        public CloudFolderDto CloudFolder { get; set; }
    }

    [AutoMapTo(typeof(CloudFile))]
    public class CreateCloudFileDto
    {
        public string PublicId { get; set; }
        public FileType FileType { get; set; }
        public string FileBase64 { get; set; }
        public bool IsOverride { get; set; }
        public long FolderId { get; set; }
    }

    public class CreateCloudinaryFileDto
    {
        public string PublicId { get; set; }
        public FileType FileType { get; set; }
        public string FileBase64 { get; set; }
        public bool IsOverride { get; set; }
        public string FolderName { get; set; }
    }

    public class UpdateCloudinaryFileDto
    {
        public string PublicId { get; set; }
        public FileType FileType { get; set; }
        public string FileBase64 { get; set; }
        public bool IsOverride { get; set; }
    }

    public class UpdateCloudFileDto : EntityDto<long>
    {
        public FileType FileType { get; set; }
        public string FileBase64 { get; set; }
        public bool IsOverride { get; set; }
    }
}
