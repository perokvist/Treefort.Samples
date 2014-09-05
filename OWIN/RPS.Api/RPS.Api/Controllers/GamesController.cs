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
        private readonly AwailibleGames _awailibleGames;

        public GamesController(ICommandBus commandBus, AwailibleGames awailibleGames)
        {
            _commandBus = commandBus;
            _awailibleGames = awailibleGames;
        }

        [HttpPost, ActionName("create")]
        public HttpResponseMessage CreateGame(JObject input)
        {
            //TODO remove xml support
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
        
        public IEnumerable<string> Get()
        {
            return _awailibleGames.Games.Select(x => x.Value);
        }
    }
}
