using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(RPS.Api.Host.Startup))]

namespace RPS.Api.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            new Api.Startup().Configuration(app, builder => {  });
        }
    }
}
