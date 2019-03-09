using Config.Lib;
using Config.Lib.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Config.API
{
    public class ConfigService : IConfigService
    {
        public ConfigRepository ConfigRepository { get; private set; }
        public ConfigService()
        {
            string connStr = "server = localhost; userid = root; password = Kartal1903; database = config";
            ConfigManager.Instance.Init("demo", connStr, 20000);
            
            ConfigRepository = new ConfigRepository(connStr);
        }

        public async Task<T> GetValue<T>(string key)
        {
            return await ConfigManager.Instance.GetValue<T>(key);
        }

    }
}
