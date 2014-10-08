using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
            var response = await client.GetAsync(_baseAddress + "api/Games/available");
            var games = await response.Content.ReadAsAsync<List<Game.ReadModel.Game>>();
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
            var response = await client.PutAsJsonAsync(_baseAddress + "api/Games/available/" + Guid.NewGuid(), new { playerName = "per2", move = "paper" });
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode); 
        }

        [Fact]
        public async void SecondRequestReturns304()
        {
            using (var client = new HttpClient())
            {
                var createResponse = await client.PostAsJsonAsync(_baseAddress + "api/Games/", new { playerName = "Wario", gameName = "CacheGame", move = "rock" });
                await Task.Delay(50);
                var availableResponse = await client.GetAsync(createResponse.Headers.Location);
                client.DefaultRequestHeaders.IfNoneMatch.Add(new EntityTagHeaderValue(availableResponse.Headers.ETag.Tag));
                var availableResponse2 = await client.GetAsync(createResponse.Headers.Location);
                Assert.Equal(HttpStatusCode.NotModified, availableResponse2.StatusCode); 
            }
        }

        [Fact]
        public async void FullGame()
        {
            using (var client = new HttpClient())
            {
                var createResponse = await client.PostAsJsonAsync(_baseAddress + "api/Games/", new { playerName = "Mario", gameName = "FullGame", move = "rock" });
                await Task.Delay(50);
                var availableResponse = await client.GetAsync(createResponse.Headers.Location);
                var game = await availableResponse.Content.ReadAsAsync<Game.ReadModel.Game>();
                
                //client.DefaultRequestHeaders.IfNoneMatch.Add(new EntityTagHeaderValue(availableResponse.Headers.ETag.Tag));

                var moveResponse = await client.PutAsJsonAsync(_baseAddress + "api/Games/available/" + game.GameId, new { playerName = "Lugi", move = "paper" });
                await Task.Delay(50);
                var endenResponse = await client.GetAsync(moveResponse.Headers.Location);
                var endGame = await endenResponse.Content.ReadAsAsync<Game.ReadModel.EndedGame>();

                Assert.Equal("PlayerTwoWin", endGame.Winner);     
            }
        }

        public void Dispose()
        {
            _host.Dispose();
        }
    }

}
