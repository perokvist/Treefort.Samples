using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;
using Xunit;

namespace RPS.Api.IT
{
    public class ApiTests : IDisposable
    {
        private readonly IDisposable _host;
        private readonly string _baseAddress;

        public ApiTests()
        {
            _baseAddress = "http://localhost:9000/";
            _host = WebApp.Start<RPS.Api.Startup>(url: _baseAddress);
        }

        [Fact]
        public async void Test()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_baseAddress + "api/Games");


        }

        public void Dispose()
        {
            _host.Dispose();
        }
    }

}
