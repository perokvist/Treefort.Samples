using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.ServiceBus;
using Owin;
using Treefort.Application;
using Treefort.Azure.Commanding;
using Treefort.Azure.Infrastructure;
using Treefort.Azure.Messaging;
using Treefort.Infrastructure;
using Treefort.Commanding;

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

            var commandDispatcher = new Dispatcher<object, Task>();
            commandDispatcher.Register<Game.CreateGameCommand>(command => Task.Run(() => GameHandler.handle(command)));

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //Azure config
            //var bus = GetAzureCommandBus();
            
            //Local config
            var bus = new ApplicationServer(commandDispatcher.Dispatch, new ConsoleLogger()); 

            config.DependencyResolver = new ServiceResolver(
                new StaticScope()
                .Add<ICommandBus>(bus)
                );

            app.UseWebApi(config);

            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Hello, world.");
            });
        }

        private static ICommandBus GetAzureCommandBus()
        {
            var connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            var manager = NamespaceManager.CreateFromConnectionString(connectionString);
            const string path = "commands";
            if (!manager.QueueExists(path))
                manager.CreateQueue(path);
            return new CommandBus(new QueueSender(connectionString, path), new JsonTextSerializer());
        }
    }
}
