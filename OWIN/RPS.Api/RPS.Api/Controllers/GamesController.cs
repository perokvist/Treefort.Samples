using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RPS.Api.PublicDomain;
using Treefort.Commanding;
using Treefort.Common;
using RPS.Game.Domain;

namespace RPS.Api.Controllers
{

    [RoutePrefix("api/Games")]
    public class GamesController : ApiController
    {
        private readonly ICommandBus _commandBus;
        private readonly IReadService _readService;

        public GamesController(ICommandBus commandBus, IReadService readService)
        {
            _commandBus = commandBus;
            _readService = readService;
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Create(PublicDomain.CreateGameCommand input)
        {
            var gameId = Guid.NewGuid();

            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            Move move;
            if (!Enum.TryParse(input.Move, true, out move))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid move");

            var command = new RPS.Game.Domain.CreateGameCommand(gameId, input.PlayerName, input.PlayerName, move);
            _commandBus.SendAsync(command); //Note - fire and forget with app server 

            return Request.CreateResponse(HttpStatusCode.Accepted)
                .Tap(message => message.Headers.Location = new Uri(Url.Link(RouteConfiguration.AwailableGamesRoute,  new {})));
        }

        [HttpPut]
        [Route("awailable/{id:Guid}")]
        public HttpResponseMessage Move(Guid id, PublicDomain.MakeMoveCommand input)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            Move move;
            if (!Enum.TryParse(input.Move, true, out move))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid move");

            var command = new RPS.Game.Domain.MakeMoveCommand(id, move, input.PlayerName);

            _commandBus.SendAsync(command);
            return Request.CreateResponse(HttpStatusCode.Accepted).Tap(
                    r => r.Headers.Location = new Uri(Url.Link(RouteConfiguration.EndedGamesRoute, new { })));
        }

        
        [Route("awailable", Name = RouteConfiguration.AwailableGamesRoute)]
        public IEnumerable<Game> GetAwailable()
        {
            return _readService
                .AwailableGames;
        }
        
        [Route("ended", Name = RouteConfiguration.EndedGamesRoute)]
        public IEnumerable<EndedGame> GetEnded()
        {
            return _readService.EndedGames;
        }    

    }

}
