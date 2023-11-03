using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LibrarySystem.Entities;

namespace LibrarySystem.Managers.CloudFolders.Dtos
{
    [AutoMapTo(typeof(CloudFolder))]
    public class CloudFolderDto : EntityDto<long>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int Level { get; set; }
        public bool IsLeaf { get; set; }
        public long? ParentId { get; set; }
        public string Code { get; set; }
        public string CombineName { get; set; }
    }

    [AutoMapTo(typeof(CloudFolder))]
    public class CreateCloudFolderDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public long? ParentId { get; set; }
    }

    [AutoMapTo(typeof(CloudFolder))]
    public class UpdateCloudFolderDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class InputToGetFolderDto
    {
        public bool? IsActive { get; set; }
        public bool? IsLeaf { get; set; }
        public string? SearchText { get; set; }

        public bool IsGetAll()
        {
            return !IsActive.HasValue && string.IsNullOrEmpty(SearchText) && !IsLeaf.HasValue;
        }
    }
}
