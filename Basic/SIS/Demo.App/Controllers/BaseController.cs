using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Result;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Demo.App.Controllers
{
    public abstract class BaseController
    {
        protected IHttpRequest Request { get; set; }

        private bool IsLoggedIn()
        {
            return this.Request.Session.ContainsParameter("username");
        }

        private string ParseTemplate(string viewContent)
        {
            if (this.IsLoggedIn())
            {
                return viewContent.Replace("@Model.HelloMessage", $"Hello, {this.Request.Session.GetParamenter("username")}");
            }
            else
            {
                return viewContent.Replace("@Model.HelloMessage", "Hello World!");
            }           
        }

        public IHttpResponse View([CallerMemberName] string view = null)
        {
            string controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            string viewName = view;

            string viewContent = File.ReadAllText("Views/" + controllerName 
                + "/" + viewName + ".html");

            viewContent = this.ParseTemplate(viewContent);

            HtmlResult htmlResult = new HtmlResult(viewContent, HttpResponseStatusCode.Ok);

            return htmlResult;
        }

        public IHttpResponse Redirect(string url)
        {
            return new RedirectResult(url);
        }
    }
}
