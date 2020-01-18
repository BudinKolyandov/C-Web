
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Result;
using System.Collections.Generic;

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

        public ActionResult Json(IHttpRequest httpRequest)
        {
            return Json(new List<object>() { 
            new
            {
                Name = "Pesho",
                Age = 28,
                Ocupation = "Bezraboten"
            },
            new
            {
                Name = "Pesho",
                Age = 28,
                Ocupation = "Bezraboten"
            },
            new
            {
                Name = "Pesho",
                Age = 28,
                Ocupation = "Bezraboten"
            },
            });
        }

        public ActionResult File(IHttpRequest httpRequest)
        {
            string folderLocation = "/../";
            string assemblyLocation = this.GetType().Assembly.Location;
            string requestedFolder = "Resources/";
            string requesttedResourse = httpRequest.QueryData["file"].ToString();

            string fullPathToResourse = assemblyLocation + folderLocation + requestedFolder + requesttedResourse;

            if (System.IO.File.Exists(fullPathToResourse))
            {
                byte[] content = System.IO.File.ReadAllBytes(fullPathToResourse);
                return FileResult(content);
            }

            return NotFound();
        }
    }
}
