using Config.Lib.Models;
using Config.Lib.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Config.Lib
{

    public sealed class ConfigManager
    {
        private static readonly Lazy<ConfigManager>
           lazy = new Lazy<ConfigManager>(() => new ConfigManager());

        public static ConfigManager Instance { get { return lazy.Value; } }

        private string connectionString;
        private int refreshTimerIntervalInMs;
        private string applicationName;
        private bool IsInitialized = false;
        private ConfigRepository configRepo = null;
        private Dictionary<string, List<ConfigModel>> configCache;


        public void Init(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            this.connectionString = connectionString;
            this.refreshTimerIntervalInMs = refreshTimerIntervalInMs;
            this.applicationName = applicationName;
            configCache = new Dictionary<string, List<ConfigModel>>();
            configRepo = new ConfigRepository(connectionString);
            if (IsInitialized)
            {
                return;
            }
            StartReadingConfigValues();
        }


        public async Task<T> GetValue<T>(string key)
        {
            try
            {
                //interval süresi dolmadan db ye kayıt eklenirse ve eş zamanlı olarak
                //değer alınmaya çalışılırsa cache de olmayacağndan db den oku
                //cachede varsa cacheden oku
                var configModel = configCache.ContainsKey(applicationName) ? configCache[applicationName].FirstOrDefault(p => p.Name.ToLower() == key.ToLower()) : null;
                if (configModel == null)
                {

                    configModel = await configRepo.ReadConfig(applicationName, key);
                    if (configModel == null)
                    {
                        return default(T);
                    }
                }

                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    // Cast ConvertFromString(string text) : object to (T)
                    return (T)converter.ConvertFromString(configModel.Value);
                }

                return default(T);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        void StartReadingConfigValues()
        {
            IsInitialized = true;

            Console.WriteLine("başladım");
            Func<Exception, IObservable<List<ConfigModel>>> exceptionHandler = exp =>
            {

                Console.WriteLine("hata " + exp.Message);
                List<ConfigModel> list = null;
                if (configCache.ContainsKey(applicationName))
                {
                    list = configCache[applicationName];
                }
                else
                {
                    list = new List<ConfigModel>();
                    configCache.Add(applicationName, list);
                }

                return Observable.Return(list);
            };

            var dbCall = Observable.FromAsync(() => configRepo.ReadAllConfig(this.applicationName))
                    .Timeout(TimeSpan.FromMilliseconds(this.refreshTimerIntervalInMs - 100))
                    .Catch(exceptionHandler);

            Observable.Interval(TimeSpan.FromMilliseconds(this.refreshTimerIntervalInMs))
                .StartWith(0)
                .Select(p => dbCall)
                .SelectMany(p => p)
                .Subscribe(x =>
                {
                    if (configCache.ContainsKey(this.applicationName))
                    {
                        configCache[this.applicationName] = x;
                    }
                    else
                    {
                        configCache.Add(this.applicationName, x);
                    }

                    Console.WriteLine(x.Count + "   " + DateTime.Now + " - " + JsonConvert.SerializeObject(x));
                });
        }
    }
}
