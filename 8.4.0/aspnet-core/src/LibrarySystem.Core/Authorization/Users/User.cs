using System;
using System.Collections.Generic;
using Abp.Authorization.Users;
using Abp.Extensions;
using LibrarySystem.Constants.Enum;

namespace LibrarySystem.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public override string FullName => Surname + " " + Name;
        public string AvatarPath { get; set; }
        public DateTime? DOB { get; set; }
        public JobType Job { get; set; }
        public string? Address { get; set; }

        public const string DefaultPassword = "123qwe";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
