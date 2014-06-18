using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using RPS.Api.Extensions;
using Treefort.Commanding;
using Treefort.Common;

namespace RPS.Api.Controllers
{
    public class GamesController : ApiController
    {
        private readonly ICommandBus _commandBus;

        public GamesController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        [HttpPost, ActionName("create")]
        public async Task<HttpResponseMessage> CreateGameAsync(JObject input)
        {
            //TODO remove xml support
            var gameId = Guid.NewGuid();
            var cmd = new CommandAdapter<Game.CreateGameCommand>
                (new RPS.Game.CreateGameCommand(input.Value<string>("playerName"), input.ToMove(), input.Value<string>("gameName"), gameId.ToString()))
            ;
            await _commandBus.SendAsync(cmd); //Not fire and forget 
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
        
        public HttpResponseMessage Get()
        {
            //TODO no projections/views
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Hello") };
        }
    }
}
