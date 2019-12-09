using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;
using SIS.WebServer.Routing.Contracts;
using System;
using System.Net.Sockets;

namespace SIS.WebServer
{
    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly IServerRoutingTable serverRoutingTable;


        public ConnectionHandler(Socket client, IServerRoutingTable serverRoutingTable)
        {
            CoreValidator.ThrowIfNull(client, nameof(client));
            CoreValidator.ThrowIfNull(serverRoutingTable, nameof(serverRoutingTable));

            this.client = client;
            this.serverRoutingTable = serverRoutingTable;
        }

        public void PrepareResponse(IHttpResponse httpResponce)
        {
            //TODO
            throw new NotImplementedException();
        }

        public IHttpRequest ReadRequest()
        {
            //TODO
            return null;
        }

        public IHttpResponse HandleRequest(IHttpRequest httpRequest)
        {
            //TODO
            return null;
        }

        public void ProcessRequest()
        {
            IHttpResponse result = null;
            try
            {
                var httpRequest = this.ReadRequest();

                if (httpRequest != null)
                {
                    Console.WriteLine($"Processing: {httpRequest.RequestMethod} {httpRequest.Path}");

                    var httpResponse = this.HandleRequest(httpRequest);

                    this.PrepareResponse(httpResponse);
                }
            }
            catch (BadRequestException e)
            {
                result = new TextResult(e.Message, HttpResponseStatusCode.BadReqest);
            }
            catch (Exception e)
            {
                result = new TextResult(e.Message, HttpResponseStatusCode.InternalServerError);
            }
        }


    }
}
