using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LibrarySystem.CoreDependencies.Annotations;
using LibrarySystem.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Managers.Authors.Dtos
{
    [AutoMapTo(typeof(Author))]
    public class AuthorDto : EntityDto<long>
    {
        [ApplySearchAttribute]
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string ShortBio { get; set; }
    }

    [AutoMapTo(typeof(Author))]
    public class CreateAuthorDto
    {
        [Required]
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string ShortBio { get; set; }
    }

    [AutoMapTo(typeof(Author))]
    public class UpdateAuthorDto : EntityDto<long>
    {
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string ShortBio { get; set; }
    }
}
