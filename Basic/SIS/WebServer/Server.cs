using SIS.HTTP.Common;
using SIS.WebServer.Routing.Contracts;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SIS.WebServer
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

            this.listener = new TcpListener(IPAddress.Parse(LocalhostIpAddress), port);
        }

        private async Task Listen(Socket client)
        {
            var connectionHandler = new ConnectionHandler(client, this.serverRoutingTable);
            await connectionHandler.ProcessRequestAsync();
        }

        public void Run()
        {
            this.listener.Start();
            this.isRunning = true;
            System.Console.WriteLine($"Server started at http://{LocalhostIpAddress}:{port}");

            while (this.isRunning)
            {
                System.Console.WriteLine("Waiting for client...");
                var client = this.listener.AcceptSocketAsync().GetAwaiter().GetResult();
                Task.Run(() => this.Listen(client));
            }
        }


    }
}
