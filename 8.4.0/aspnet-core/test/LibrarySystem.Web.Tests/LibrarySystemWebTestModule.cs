using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using LibrarySystem.EntityFrameworkCore;
using LibrarySystem.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace LibrarySystem.Web.Tests
{
    [DependsOn(
        typeof(LibrarySystemWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class LibrarySystemWebTestModule : AbpModule
    {
        public LibrarySystemWebTestModule(LibrarySystemEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LibrarySystemWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(LibrarySystemWebMvcModule).Assembly);
        }
    }
}