using SIS.HTTP.Common;
using SIS.MvcFramework.Routing;
using SIS.MvcFramework.Sessions;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SIS.MvcFramework
{
    public class Server
    {
        private const string LocalhostIpAddress = "127.0.0.1";

        private readonly int port;

        private readonly TcpListener listener;

        private readonly IServerRoutingTable serverRoutingTable;

        private bool isRunning;

        public Server(int port, IServerRoutingTable serverRoutingTable)
        {
            CoreValidator.ThrowIfNull(serverRoutingTable, nameof(serverRoutingTable));

            this.port = port;
            this.serverRoutingTable = serverRoutingTable;

            listener = new TcpListener(IPAddress.Parse(LocalhostIpAddress), port);
        }

        private async Task Listen(Socket client)
        {
            var connectionHandler = new ConnectionHandler(client, this.serverRoutingTable);
            await connectionHandler.ProcessRequestAsync();
        }

        public void Run()
        {
            listener.Start();
            isRunning = true;
            System.Console.WriteLine($"Server started at http://{LocalhostIpAddress}:{port}");

            while (isRunning)
            {
                var client = listener.AcceptSocketAsync().GetAwaiter().GetResult();
                Task.Run(() => Listen(client));
            }
        }


    }
}
