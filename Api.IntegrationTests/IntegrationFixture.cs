using System;
using System.Net.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace ProjectManagement.Api.IntegrationTests
{
    public class IntegrationFixture : IDisposable
    {
        public HttpClient Client { get; private set; }

        private TestServer _server;

        public IntegrationFixture()
        {
            IWebHostBuilder webBuilder = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();

            _server = new TestServer(webBuilder);

            Client = _server.CreateClient();
        }

        public void Dispose()
        {
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