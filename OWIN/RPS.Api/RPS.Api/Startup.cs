using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;
using RPS.Api.Controllers;
using RPS.Game.Domain;
using Treefort.Application;
using Treefort.Events;
using Treefort.Infrastructure;
using Treefort.Commanding;
using Treefort.Read;

[assembly: OwinStartup(typeof(RPS.Api.Startup))]

namespace RPS.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //Azure config
            //var bus = GetAzureCommandBus();

            //Local config
            var commandDispatcher = new Dispatcher<ICommand, Task>();

            var games = new AllGames();
            var projectionsEventListener = new ProjectionEventListener(new List<IProjection> { games});
            var eventPublisher = new EventPublisher(new List<IEventListener> { projectionsEventListener }, Console.WriteLine);
            var eventStore = new PublishingEventStore(new InMemoryEventStore(() => new InMemoryEventStream()), eventPublisher);

            commandDispatcher.Register<IGameCommand>(
                command => ApplicationService.UpdateAsync<Game.Domain.Game, GameState>(
                    state => new Game.Domain.Game(state), eventStore, command, game => game.Handle(command)));

            var bus = new ApplicationServer(commandDispatcher.Dispatch, new ConsoleLogger());

            var cb = new ContainerBuilder();
            cb.RegisterInstance(bus).AsImplementedInterfaces();
            cb.RegisterInstance(games).AsSelf();
            cb.RegisterApiControllers(Assembly.GetExecutingAssembly());

            config.DependencyResolver = new AutofacWebApiDependencyResolver(cb.Build());

            app.UseWebApi(config);

            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Welcome to RPS APi.");
            });
        }

        //private static ICommandBus GetAzureCommandBus()
        //{
        //    var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
        //    var manager = NamespaceManager.CreateFromConnectionString(connectionString);
        //    const string path = "commands";
        //    if (!manager.QueueExists(path))
        //        manager.CreateQueue(path);
        //    return new CommandBus(new QueueSender(connectionString, path), new JsonTextSerializer());
        //}
    }
}
