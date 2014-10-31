using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;

[assembly: OwinStartup(typeof(RPS.Api.IISHost.IisStartup))]

namespace RPS.Api.IISHost
{
    public class IisStartup
    {
        public void Configuration(IAppBuilder app)
        {
            new Startup().Configuration(app, builder => builder.UseStageMarker(PipelineStage.MapHandler));
        }
    }
}
