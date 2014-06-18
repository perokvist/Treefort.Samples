using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace RPS.Processor.Role
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.TraceInformation("RPS.Processor.Role entry point called");
            Bootstrapper.Start(ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"]);
            base.Run();

        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            return base.OnStart();
        }
    }
}
