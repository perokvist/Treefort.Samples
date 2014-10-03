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
        public async void GamesReturnsProjection()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_baseAddress + "api/Games/awailable");
            var games = await response.Content.ReadAsAsync<List<Game>>();
            Assert.Empty(games);
        }

        [Fact]
        public async void CreateGameReturnAccepted()
        {
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync(_baseAddress + "api/Games/", new { playerName ="per", gameName = "testGame", move = "paper"});
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async void MakeMoveReturnsAccepted()
        {
            var client = new HttpClient();
            var response = await client.PutAsJsonAsync(_baseAddress + "api/Games/awailable/" + Guid.NewGuid().ToString(), new { playerName = "per2", move = "paper" });
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode); 
        }

        public void Dispose()
        {
            _host.Dispose();
        }
    }

}
