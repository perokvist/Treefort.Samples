using System;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.ApplicationInsights;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using RPS.Game.Domain;
using RPS.Game.ReadModel;
using Treefort.Application;
using Treefort.Azure.Infrastructure;
using Treefort.Azure.Messaging;
using Treefort.Events;
using Treefort.Infrastructure;
using Treefort.Commanding;
using Treefort.Read;
using CacheCow.Server;
using Treefort.Azure.Commanding;
using Treefort.Messaging;
using CreateGameCommand = RPS.Game.Domain.CreateGameCommand;

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
        public void Configuration(IAppBuilder app, Action<IAppBuilder> stageMakerHook)
        {

            app.UseFileServer(new FileServerOptions() { FileSystem = new PhysicalFileSystem("site") });

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new NullJsonHandler());

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            var tc = new TelemetryClient();

            var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];

            //Local config
            var commandDispatcher = new Dispatcher<ICommand, Task>();
            var commandBus = new CommandBus(new QueueSender(connectionString, "commands-queue"), new JsonTextSerializer());

            var awailableGames = new AvailableGames();
            var endedGames = new EndendGames();

            var eventPublisher = new EventPublisher(Console.WriteLine, new ProjectionEventListener(awailableGames, endedGames));
            var eventStore = new PublishingEventStore(new InMemoryEventStore(() => new InMemoryEventStream()), eventPublisher);

            commandDispatcher.Register<IGameCommand>(
                command => ApplicationService.UpdateAsync<Game.Domain.Game, GameState>(
                    state => new Game.Domain.Game(state), eventStore, command, game => game.Handle(command)));

            var bus = new CommandBusAction(cmd =>
            {
                tc.TrackEvent(string.Format("Dispatching {0}", cmd.GetType()));
                return commandDispatcher.Dispatch(cmd);
            });

            var multiBus = new MultiCommandBus(commandBus, bus);

            var cb = new ContainerBuilder();
            cb.RegisterInstance(multiBus).AsImplementedInterfaces();
            cb.RegisterType<ReadService>().AsImplementedInterfaces().SingleInstance();
            cb.RegisterInstance(awailableGames).AsSelf();
            cb.RegisterInstance(endedGames).AsSelf();
            cb.RegisterApiControllers(Assembly.GetExecutingAssembly());

            config.DependencyResolver = new AutofacWebApiDependencyResolver(cb.Build());

            config.MessageHandlers.Add(new CachingHandler(config, new InMemoryEntityTagStore()));
            app.UseWebApi(config);
            stageMakerHook(app);

            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Welcome to RPS APi.");
            });
        }

    }
}
