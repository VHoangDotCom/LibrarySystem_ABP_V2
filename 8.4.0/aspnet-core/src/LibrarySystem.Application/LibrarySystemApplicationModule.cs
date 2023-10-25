using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using LibrarySystem.Authorization;

namespace LibrarySystem
{
    [DependsOn(
        typeof(LibrarySystemCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class LibrarySystemApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<LibrarySystemAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(LibrarySystemApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
