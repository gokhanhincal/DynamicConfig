using System.Collections.Generic;
using System.Threading.Tasks;
using Config.Lib.Models;

namespace Config.Lib.Repositories
{
    public interface IConfigRepository
    {
        Task AddConfig(ConfigModel model);
        Task DeleteConfig(long id);
        Task<List<ConfigModel>> ReadAllConfig(string applicationName);
        Task<ConfigModel> ReadConfig(string applicationName, string key);
        Task UpdateConfig(ConfigModel model);
    }
}