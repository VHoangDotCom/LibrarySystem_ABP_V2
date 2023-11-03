using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace LibrarySystem.Entities
{
    public class CloudFolder : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int Level { get; set; }
        public bool IsLeaf { get; set; }
        public long? ParentId { get; set; }
        public string Code { get; set; }
        public string CombineName { get; set; }
        public virtual ICollection<CloudFile> CloudFiles { get; set; }
    }
}
