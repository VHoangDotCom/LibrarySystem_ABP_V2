using Abp.Authorization;
using LibrarySystem.Authorization.Roles;
using LibrarySystem.Authorization.Users;

namespace LibrarySystem.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
