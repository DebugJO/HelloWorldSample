using MyAppLib.AppStates;
using System.Threading.Tasks;

namespace MyAppLib.Services;

public interface IConfigService
{
    Task<AppConfigState> Load();

    Task Save(AppConfigState appConfig);
}