using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Mimo.Persistence.DbContexts;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Mimo.Api.IntegrationTests.Infrastructure
{
    public abstract class AbstractScenario : IClassFixture<TestServerFixture>, IDisposable
    {
        protected readonly HttpClient httpClient;
        protected readonly HttpClient contentCreatorHttpClient;
        protected readonly HttpMessageHandler handler;
        protected readonly TestServer server;
        protected readonly IServiceProvider services;
        protected readonly IMimoDbContext dbContext;

        protected AbstractScenario(TestServerFixture testServerFixture)
        {
            httpClient = testServerFixture.HttpClient;
            contentCreatorHttpClient = testServerFixture.ContentCreatorHttpClient;
            handler = testServerFixture.Handler;
            server = testServerFixture.Server;
            services = testServerFixture.Services;
            dbContext = services.GetService<IMimoDbContext>();
        }


        protected T GetService<T>() => (T)services.GetService(typeof(T));

        protected StringContent BuildJsonContent(object objectToSerialize)
        {
            return new StringContent(JsonConvert.SerializeObject(objectToSerialize), Encoding.UTF8, "application/json");
        }

        public string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public virtual void Dispose()
        {
            //tear down
        }
    }
}
