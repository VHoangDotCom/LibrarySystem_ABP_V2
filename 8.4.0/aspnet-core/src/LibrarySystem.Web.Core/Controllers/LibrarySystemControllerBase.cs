using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.Controllers
{
    public abstract class LibrarySystemControllerBase: AbpController
    {
        protected LibrarySystemControllerBase()
        {
            LocalizationSourceName = LibrarySystemConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
