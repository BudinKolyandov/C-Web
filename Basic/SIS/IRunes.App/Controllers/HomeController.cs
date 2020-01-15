
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.MvcFramework;

namespace IRunes.App.Controllers
{
    public class HomeController : Controller
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            if (this.IsLoggedIn(request))
            {
                this.ViewData["Username"] = request.Session.GetParamenter("username");
                return this.View("Index-Logged");
            }
            return this.View();
        }

    }
}
