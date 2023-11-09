using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using LibrarySystem.Authorization.Users;
using LibrarySystem.Constants.Enum;

namespace LibrarySystem.Users.Dto
{
    [AutoMapTo(typeof(User))]
    public class CreateUserDto : IShouldNormalize
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string[] RoleNames { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public DateTime? DOB { get; set; }
        public JobType Job { get; set; }
        public string Address { get; set; }
        public FileType? AvatarFileType { get; set; }
        public string AvatarBase64 { get; set; }

        public void Normalize()
        {
            if (RoleNames == null)
            {
                RoleNames = new string[0];
            }
        }
    }

    [AutoMapFrom(typeof(User))]
    public class UpdateUserDto : EntityDto<long>
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string FullName { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }

        public string[] RoleNames { get; set; }
        public DateTime? DOB { get; set; }
        public JobType Job { get; set; }
        public string Address { get; set; }
        public FileType? AvatarFileType { get; set; }
        public string AvatarBase64 { get; set; }
    }
}
