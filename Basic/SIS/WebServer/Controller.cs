using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.MvcFramework.Extensions;
using SIS.MvcFramework.Identity;
using SIS.MvcFramework.Result;
using SIS.MvcFramework.ViewEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace SIS.MvcFramework
{
    public abstract class Controller
    {
        private readonly IviewEngine viewEngine;

        protected Controller()
        {
            this.viewEngine = new SisViewEngine();
        }

        public Principal User =>
            this.Request.Session.ContainsParameter("principal")
            ? (Principal) this.Request.Session.GetParamenter("principal")
            : null;

        public IHttpRequest Request { get; set; }
        
        protected bool IsLoggedIn()
        {
            return this.Request.Session.ContainsParameter("principal");
        }

        protected void SignIn(Guid id, string username, string email)
        {
            this.Request.Session.AddParameter("principal", new Principal{
                Id = id,
                Username= username,
                Email = email
            });
            
        }

        protected void SignOut()
        {
            this.Request.Session.ClearParameters();
        }

        protected  ActionResult View([CallerMemberName] string view = null)
        {
            return this.View<object>(null, view);
        }

        protected ActionResult View<T>(T model = null, [CallerMemberName] string view = null)
            where T : class
        {
            string controllerName = GetType().Name.Replace("Controller", string.Empty);
            string viewName = view;

            string viewContent = File.ReadAllText("Views/" + controllerName
                + "/" + viewName + ".html");
            viewContent = this.viewEngine.GetHtml(viewContent, model);

            string layoutContent = File.ReadAllText("Views/_Layout.html");
            layoutContent = this.viewEngine.GetHtml(layoutContent, model);
            layoutContent = layoutContent.Replace("@RenderBody()", viewContent);

            HtmlResult htmlResult = new HtmlResult(layoutContent);

            return htmlResult;
        }

        protected ActionResult Redirect(string url)
        {
            return new RedirectResult(url);
        }

        protected ActionResult Xml(object obj)
        {
            return new XmlResult(obj.ToXml());
        }

        protected ActionResult Json(object obj)
        {
            return new JsonResult(obj.ToJson());
        }

        protected ActionResult FileResult(byte[] fileContent)
        {
            return new FileResult(fileContent);
        }

        protected ActionResult NotFound(string message = "")
        {
            return new NotFoundResult(message);
        }

    }
}
