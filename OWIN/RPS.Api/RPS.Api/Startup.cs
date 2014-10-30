using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using RPS.Game.Domain;
using RPS.Game.ReadModel;
using Treefort.Application;
using Treefort.Events;
using Treefort.Infrastructure;
using Treefort.Commanding;
using Treefort.Read;
using CacheCow.Server;
using CacheCow.Common;

[assembly: OwinStartup(typeof(RPS.Api.Startup))]

namespace RPS.Api
{
    public static class RouteConfiguration
    {
        public const string GamesRoute = "Games";
        public const string AvailableGamesRoute = "Available";
        public const string EndedGamesRoute = "Ended";

    }
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            app.UseFileServer(new FileServerOptions() { FileSystem = new PhysicalFileSystem("site") });

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new NullJsonHandler());

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //Local config
            var commandDispatcher = new Dispatcher<ICommand, Task>();
            
            var awailableGames = new AvailableGames();
            var endedGames = new EndendGames();

            var eventPublisher = new EventPublisher(Console.WriteLine, new ProjectionEventListener(awailableGames, endedGames));
            var eventStore = new PublishingEventStore(new InMemoryEventStore(() => new InMemoryEventStream()), eventPublisher);

            commandDispatcher.Register<IGameCommand>(
                command => ApplicationService.UpdateAsync<Game.Domain.Game, GameState>(
                    state => new Game.Domain.Game(state), eventStore, command, game => game.Handle(command)));

            var bus = new ApplicationServer(commandDispatcher.Dispatch, new ConsoleLogger());

            var cb = new ContainerBuilder();
            cb.RegisterInstance(bus).AsImplementedInterfaces();
            cb.RegisterType<ReadService>().AsImplementedInterfaces().SingleInstance();
            cb.RegisterInstance(awailableGames).AsSelf();
            cb.RegisterInstance(endedGames).AsSelf();
            cb.RegisterApiControllers(Assembly.GetExecutingAssembly());

            config.DependencyResolver = new AutofacWebApiDependencyResolver(cb.Build());

            config.MessageHandlers.Add(new CachingHandler(config, new InMemoryEntityTagStore()));
            app.UseWebApi(config);

            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Welcome to RPS APi.");
            });
        }

    }
}
