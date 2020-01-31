using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ProjectManagement.Api
{
    public class Program
    {
        private static readonly string _storeFilename = "datastore.json";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(AddAppConfiguration)
                .UseStartup<Startup>();
        }

        public static void AddAppConfiguration(IConfigurationBuilder config)
        {
            var configs = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("datastoreFilename", _storeFilename)
            };

            config.AddInMemoryCollection(configs);
        }
    }
}