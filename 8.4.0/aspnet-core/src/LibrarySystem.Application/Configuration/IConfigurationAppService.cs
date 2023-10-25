using System.Threading.Tasks;
using LibrarySystem.Configuration.Dto;

namespace LibrarySystem.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
