using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using RPS.Api.Extensions;
using Treefort.Commanding;
using Treefort.Common;
using Treefort.Messaging;
using RPS.Game.Domain;

namespace RPS.Api.Controllers
{
    public class GamesController : ApiController
    {
        private readonly ICommandBus _commandBus;
        private readonly AllGames _awailibleGames;

        public GamesController(ICommandBus commandBus, AllGames awailibleGames)
        {
            _commandBus = commandBus;
            _awailibleGames = awailibleGames;
        }
        
        [HttpPost, ActionName("create")]
        public HttpResponseMessage CreateGame(JObject input)
        {
            var gameId = Guid.NewGuid();
            var cmd = new CreateGameCommand(gameId, input.Value<string>("playerName"), input.Value<string>("gameName"), input.ToMove());
            _commandBus.SendAsync(cmd); //Note - fire and forget with app server 
            return Request.CreateResponse(HttpStatusCode.Accepted)
                .Tap(message => message.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = gameId })));
        }

        [HttpPost, ActionName("move")]
        public async Task<HttpResponseMessage> MoveAsync([FromUri]Guid id, JObject input)
        {
            await _commandBus.SendAsync(null);
            return Request.CreateResponse(HttpStatusCode.Accepted).Tap(
                    r => r.Headers.Location = new Uri(Url.Link("DefaultApi", new { id })));
        }

        public IHttpActionResult Get(Guid id)
        {
            var game = _awailibleGames
                .Games.SingleOrDefault(x => x.Key == id);

            if (game.IsDefault())
                return NotFound();

            return Ok(game.Value);
        }

        public IEnumerable<string> Get()
        {
            return _awailibleGames
                .Games
                .Select(x => x.Value)
                .ToList();
        }
    }
}
