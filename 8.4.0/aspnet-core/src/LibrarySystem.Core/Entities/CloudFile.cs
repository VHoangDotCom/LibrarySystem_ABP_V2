using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using LibrarySystem.Constants.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Entities
{
    public class CloudFile : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string PublicId { get; set; }
        public FileType FileType { get; set; }
        public string ImageURL { get; set; }
        public string FileBase64 { get; set; }
        public bool IsOverride { get; set; }
        public string FolderPath { get; set; }

        [ForeignKey(nameof(FolderId))]
        public CloudFolder CloudFolder { get; set; }
        public long FolderId { get; set; }
    }
}
