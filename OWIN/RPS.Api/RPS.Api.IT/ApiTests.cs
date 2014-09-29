using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;
using Xunit;
using System.Diagnostics;

namespace RPS.Api.IT
{
    public class ApiTests : IDisposable
    {
        private readonly IDisposable _host;
        private readonly string _baseAddress;

        public ApiTests()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            _baseAddress = "http://localhost:9000/";
            _host = WebApp.Start<RPS.Api.Startup>(url: _baseAddress);
        }

        [Fact]
        public async void Test()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_baseAddress + "api/Games");
            var games = await response.Content.ReadAsAsync<List<string>>();
            Assert.Empty(games);
        }

        public void Dispose()
        {
            _host.Dispose();
        }
    }

}
