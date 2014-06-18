using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding.Binders;
using Microsoft.Owin;
using Owin;
using Treefort.Application;
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
            config.DependencyResolver = new ServiceResolver(
                new StaticScope()
                .Add(new ApplicationServer(commandDispatcher.Dispatch, new ConsoleLogger()))
                );

            app.UseWebApi(config);

            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Hello, world.");
            });
        }
    }

    public class CommandAdapter<T> :  ICommand
    {
        public CommandAdapter(T innerCommand)
        {
            
        }
        public System.Guid AggregateId
        {
            get { throw new System.NotImplementedException(); }
        }

        public System.Guid CorrelationId
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
