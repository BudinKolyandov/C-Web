
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;

namespace IRunes.App.Controllers
{
    public class HomeController : BaseController
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
