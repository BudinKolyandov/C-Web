
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;

namespace IRunes.App.Controllers
{
    public class InfoController : Controller
    {
        [NonAction]
        public override string ToString()
        {
            return base.ToString();
        }

        public IHttpResponse About(IHttpRequest httpRequest)
        {
            return this.View();
        }
    }
}
