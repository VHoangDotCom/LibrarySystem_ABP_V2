using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;

namespace LibrarySystem.Entities
{
    public class Author : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Name { get; private set; }
        public DateTime? BirthDate { get; set; }
        public string ShortBio { get; set; }
    }
}
