using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Action;
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

        public ActionResult About()
        {
            return this.View();
        }

        public ActionResult Json()
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

        public ActionResult File()
        {
            string folderLocation = "/../";
            string assemblyLocation = this.GetType().Assembly.Location;
            string requestedFolder = "Resources/";
            string requesttedResourse = this.Request.QueryData["file"].ToString();

            string fullPathToResourse = assemblyLocation + folderLocation + requestedFolder + requesttedResourse;

            if (System.IO.File.Exists(fullPathToResourse))
            {
                byte[] content = System.IO.File.ReadAllBytes(fullPathToResourse);
                return new FileResult(content);
            }

            return NotFound();
        }
    }
}
