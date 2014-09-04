using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using RPS.Api.Extensions;
using Treefort.Commanding;
using Treefort.Common;
using Treefort.Messaging;

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
        public HttpResponseMessage CreateGame(JObject input)
        {
            //TODO remove xml support
            var gameId = Guid.NewGuid();
            var cmd = new RPS.Commands.CreateGameCommand(input.Value<string>("playerName"), input.ToMove(), input.Value<string>("gameName"), gameId, Guid.NewGuid());
            _commandBus.SendAsync(cmd); //Note fire and forget with app server 
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
        
        public string Get()
        {
            
            return "bu";
            //TODO no projections/views
            //return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Hello") };
        }
    }
}
