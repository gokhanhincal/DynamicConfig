using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Config.Lib.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Config.Lib.Repositories
{
    public class ConfigRepository : IConfigRepository
    {
        private string connectionStr;

        public ConfigRepository(string connectionString)
        {
            connectionStr = connectionString;
        }

        public async Task AddConfig(ConfigModel model)
        {
            using (var conn = new MySqlConnection(connectionStr))
            {
                var script = @"insert into config.setting (Name, Type, Value, ApplicationName, IsActive)
                               values(@Name, @Type, @Value, @ApplicationName, @IsActive)";
                await conn.ExecuteAsync(script, model);
            }
        }

        public async Task UpdateConfig(ConfigModel model)
        {
            using (var conn = new MySqlConnection(connectionStr))
            {
                var script = @"update config.setting set Name=@Name, Type = @Type, Value=@Value, ApplicationName=@ApplicationName, IsActive=@IsActive)
                               where Id = @Id";
                await conn.ExecuteAsync(script, model);
            }
        }

        public async Task DeleteConfig(long id)
        {
            using (var conn = new MySqlConnection(connectionStr))
            {
                var script = @"update config.setting set IsActive=0 where Id = @Id";
                await conn.ExecuteAsync(script, new { Id = id });
            }
        }

        public async Task<List<ConfigModel>> ReadAllConfig(string applicationName)
        {
            using (var conn = new MySqlConnection(connectionStr))
            {
                var script = "select * from config.setting where IsActive = @IsActive and ApplicationName = @ApplicationName";
                var result = await conn.QueryAsync<ConfigModel>(script, new { ApplicationName = applicationName, IsActive = true });
                return result.ToList();
            }
        }

        public async Task<ConfigModel> ReadConfig(string applicationName, string key)
        {
            using (var conn = new MySqlConnection(connectionStr))
            {
                var script = "select * from config.setting where IsActive = @IsActive and ApplicationName = @ApplicationName and Name = @Name";
                var result = await conn.QueryFirstOrDefaultAsync<ConfigModel>(script, new { ApplicationName = applicationName, IsActive = true, Name = key });
                return result;
            }
        }
    }
}
