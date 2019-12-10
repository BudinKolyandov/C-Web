using Demo.App.Controllers;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;
using SIS.WebServer.Routing.Contracts;
using System;
using System.Globalization;
using System.Text;

namespace Demo.App
{
    class Program
    {
        static void Main(string[] args)
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Add(HttpRequestMethod.Get, "/", httpRequest
                => new HomeController().Home(httpRequest));

            Server server = new Server(8000, serverRoutingTable);

            server.Run();

           // string request =
           //     "POST /url/asd?name=steve&id=1#fragment HTTP/1.1\r\n"
           //     + "Authorization: Basic 2394564784351\r\n"
           //     + "Date:" + DateTime.Now + "\r\n"
           //     + "Host: localhost:5000\r\n"
           //     + "\r\n"
           //     + "username=stevenson&pasword=123&pasword=562";
           //
           // HttpRequest httpRequest = new HttpRequest(request);
           //
           // HttpResponse response = new HttpResponse//(HttpResponseStatusCode.InternalServerError);
           // response.AddHeader(new HttpHeader("Host", "localhost:5000"));
           // response.AddHeader(new HttpHeader("Date", DateTime.Now.ToString//(CultureInfo.InvariantCulture)));
           //
           // response.Content = Encoding.UTF8.GetBytes("<h1>Hello world</h1>");
           //
           // Console.WriteLine(Encoding.UTF8.GetString(response.GetBytes()));
           //
           // var s = 5;

        }
    }
}
