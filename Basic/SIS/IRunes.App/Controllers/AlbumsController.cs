using IRunes.App.Extensions;
using IRunes.Data;
using IRunes.Models;
using Microsoft.EntityFrameworkCore;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Result;
using System.Collections.Generic;
using System.Linq;

namespace IRunes.App.Controllers
{
    public class AlbumsController : Controller
    {
        public ActionResult All()
        {
            if (!this.IsLoggedIn())
            {
                this.Redirect("/Users/Login");
            }

            using (var context = new RunesDbContext())
            {
                ICollection<Album> allAlbums = context.Albums.ToList();

                if (allAlbums.Count == 0)
                {
                    this.ViewData["Albums"] = "There are currently no albums.";
                }
                else
                {
                    this.ViewData["Albums"] = 
                        string.Join(string.Empty,
                        allAlbums.Select(album => album.ToHtmlAll())
                        .ToList());
                }

                return this.View();
            }
        }

        public ActionResult Create()
        {
            if (!this.IsLoggedIn())
            {
                this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost(ActionName = "Create")]
        public ActionResult CreateConfirm()
        {
            if (!this.IsLoggedIn())
            {
                this.Redirect("/Users/Login");
            }

            using (var context = new RunesDbContext())
            {
                string name = ((ISet<string>)this.Request.FormData["name"]).FirstOrDefault();
                string cover = ((ISet<string>)this.Request.FormData["cover"]).FirstOrDefault();

                Album album = new Album
                {
                    Name = name,
                    Cover = cover,
                    Price = 0M
                };
                context.Albums.Add(album);
                context.SaveChanges();
            }
            return this.Redirect("/Albums/All");
        }

        public ActionResult Details()
        {
            if (!this.IsLoggedIn())
            {
                this.Redirect("/Users/Login");
            }

            var albumId = this.Request.QueryData["id"].ToString();

            using (var context = new RunesDbContext())
            {
                Album albumFromDb = context.Albums
                    .Include(album=>album.Tracks)
                    .SingleOrDefault(album=>album.Id.ToString() == albumId);

                if (albumFromDb == null)
                {
                    return this.Redirect("/Albums/All");
                }
                this.ViewData["Album"] = albumFromDb.ToHtmlDetails();
                return this.View();
            }
           
        }

    }
}
