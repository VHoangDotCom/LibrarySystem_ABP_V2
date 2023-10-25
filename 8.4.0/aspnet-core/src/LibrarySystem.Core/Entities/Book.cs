using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using LibrarySystem.Constants.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Entities
{
    public class Book : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public Author Author { get; set; }
        public long AuthorId { get; set; }
    }
}
