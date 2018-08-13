using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Mimo.Api.IntegrationTests.Infrastructure;
using Mimo.Persistence.DbContexts;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Xunit;

namespace Mimo.Api.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        private const string ContentCreatorAuthHeader = "Basic Y29udGVudGNyZWF0b3I6aGFzaGVkcHdk";
        private const string ClientAuthHeader = "Basic bWltb3VzZXI6aGFzaGVkcHdk";

        public TestServer Server { get; }

        public HttpClient HttpClient { get; }
        public HttpClient ContentCreatorHttpClient { get; }

        public HttpMessageHandler Handler { get; }

        public IServiceProvider Services;

        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
              .UseStartup<IntegrationTestsStartup>()
              .UseSetting(WebHostDefaults.ApplicationKey, typeof(Mimo.Api.Program).GetTypeInfo().Assembly.FullName);

            Server = new TestServer(builder);

            HttpClient = Server.CreateClient();
            HttpClient.DefaultRequestHeaders.Add("Authorization", ClientAuthHeader);
            HttpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ContentCreatorHttpClient = Server.CreateClient();
            ContentCreatorHttpClient.DefaultRequestHeaders.Add("Authorization", ContentCreatorAuthHeader);
            HttpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Handler = Server.CreateHandler();

            Services = Server.Host.Services;
        }

        public void Dispose()
        {
            Server.Dispose();
            HttpClient.Dispose();
        }

    }
}
