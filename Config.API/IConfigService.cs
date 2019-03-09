using Config.Lib.Repositories;
using System.Threading.Tasks;

namespace Config.API
{
    public interface IConfigService
    {
        ConfigRepository ConfigRepository { get; }
        Task<T> GetValue<T>(string key);
    }
}