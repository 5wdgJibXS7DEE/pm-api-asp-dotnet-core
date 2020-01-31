using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using JsonFlatFileDataStore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace ProjectManagement.Api.IntegrationTests
{
    public class IntegrationFixture : IDisposable
    {
        public HttpClient Client { get; private set; }

        public IDataStore Store { get; private set; }

        private TestServer _server;

        private string _storeFilename;

        public IntegrationFixture()
        {
            _storeFilename = Guid.NewGuid() + "-store.json";

            IWebHostBuilder webBuilder = WebHost.CreateDefaultBuilder()
                .ConfigureAppConfiguration(AddAppConfiguration)
                .UseStartup<Startup>();

            // todo GSA how can I re use the TestServer for all the integration tests?
            // if I have to create one for each test because I want a new database file
            // for each test, see above .ConfigureAppConfiguration(AddAppConfiguration)
            _server = new TestServer(webBuilder);

            Client = _server.CreateClient();

            Store = (IDataStore)_server.Services.GetService(typeof(IDataStore));
        }

        private void AddAppConfiguration(IConfigurationBuilder config)
        {
            var configs = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("datastoreFilename", _storeFilename)
            };

            config.AddInMemoryCollection(configs);
        }

        public void Dispose()
        {
            if (Store != null)
            {
                Store.Dispose();
                Store = null;
            }

            try // delete the file after the store is disposed
            {
                if (File.Exists(_storeFilename))
                {
                    File.Delete(_storeFilename);
                }
            }
            catch (Exception ex)
            {
                // todo GSA call for better logs in IntegrationFixture.Dispose
                Console.WriteLine(ex.ToString());
            }
            _storeFilename = null;

            if (Client != null)
            {
                Client.Dispose();
                Client = null;
            }

            if (_server != null)
            {
                _server.Dispose();
                _server = null;
            }
        }
    }
}