namespace Andreys.App.Controllers
{
    using Andreys.Services;
    using SIS.HTTP;
    using SIS.MvcFramework;

    public class HomeController : Controller
    {
        private readonly IProductService productsService;

        public HomeController(IProductService productsService)
        {
            this.productsService = productsService;
        }

        [HttpGet("/")]
        public HttpResponse IndexSlash()
        {
            return this.Index();
        }


        public HttpResponse Index()
        {
            if (this.IsUserLoggedIn())
            {
                var allProducts = productsService.GetAll();

                return this.View(allProducts, "Home");
            }
            return this.View();
        }
    }
}
