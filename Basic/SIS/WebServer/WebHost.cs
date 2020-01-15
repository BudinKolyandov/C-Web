using SIS.MvcFramework.Routing;

namespace SIS.MvcFramework
{
    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            var serverRoutingTable = new ServerRoutingTable();

            application.ConfigureServices();
            application.Configure(serverRoutingTable);

            Server server = new Server(8000, serverRoutingTable);
            server.Run();
        }

    }
}
