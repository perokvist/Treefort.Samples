using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.ServiceBus;
using Owin;
using RPS.Api.Controllers;
using Treefort.Application;
using Treefort.Azure.Commanding;
using Treefort.Azure.Infrastructure;
using Treefort.Azure.Messaging;
using Treefort.Events;
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
            
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //Azure config
            //var bus = GetAzureCommandBus();

            //Local config
            var commandDispatcher = new Dispatcher<ICommand, Task>();

            commandDispatcher.Register<ICommand>(command =>
                Task.Run(() => RPS.GameHandler.handle(command)));
            
            var bus = new ApplicationServer(commandDispatcher.Dispatch, new ConsoleLogger()); 
           
            //Register CommandBus
            config.DependencyResolver = new ServiceResolver(
                new StaticScope()
                .Add(new GamesController(bus))
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
