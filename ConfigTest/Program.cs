using Config.Lib;
using System;

namespace ConfigTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStr = "server = localhost; userid = root; password = Kartal1903; database = config";
            var manager = ConfigManager.Instance;
            manager.Init("demo", connStr, 20000);
            manager.Init("demo", connStr, 20);
            manager.Init("demo", connStr, 20000);
            manager.GetValue<int>("demo").Wait();
            Console.Read();
        }

        
    }
}
