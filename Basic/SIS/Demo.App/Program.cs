using Demo.App.Controllers;
using SIS.HTTP.Enums;
using SIS.MvcFramework;
using SIS.MvcFramework.Routing;

namespace Demo.App
{
    class Program
    {
        static void Main(string[] args)
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Add(HttpRequestMethod.Get, "/", httpRequest
                => new HomeController().Home(httpRequest));
            serverRoutingTable.Add(HttpRequestMethod.Get, "/login", httpRequest
                => new HomeController().Login(httpRequest));
            serverRoutingTable.Add(HttpRequestMethod.Get, "/logout", httpRequest
                => new HomeController().Logout(httpRequest));

            Server server = new Server(8000, serverRoutingTable);

            server.Run();

        }
    }
}
