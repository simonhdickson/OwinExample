using Microsoft.AspNet.SignalR;
using Microsoft.Owin.StaticFiles;
using Owin;
using System.IO;
using System.Reflection;
using System.Web.Http;

namespace OwinExample
{
    public class Startup
    {
        public void Configuration (IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("default", "{controller}");

            var signalrConfig = new HubConfiguration();

            string staticFilesDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Web");

            app.UseStaticFiles("Web");
            app.MapSignalR(signalrConfig);
            app.UseWebApi(config);
            app.UseHandlerAsync((req, res) =>
            {
                res.ContentType = "text/plain";
                return res.WriteAsync("Hello, World!");
            });
        }
    }

    public class ValueController : ApiController
    {
        public int[] GetValues()
        {
            return new[] { 1, 2, 3 };
        }
    }

    public class Move : Hub
    {
        public void Action(int x, int y)
        {
            Clients.Others.shapemoved(x, y);
        }
    }
}