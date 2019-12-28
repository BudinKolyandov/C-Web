﻿using IRunes.App.Extensions;
using IRunes.Data;
using IRunes.Models;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRunes.App.Controllers
{
    public class TracksController : BaseController
    {
        public IHttpResponse Create(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                this.Redirect("/Users/Login");
            }
            string albumId = httpRequest.QueryData["albumId"].ToString();

            this.ViewData["AlbumId"] = albumId;
            return this.View();
        }

        public IHttpResponse CreateConfirm(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                this.Redirect("/Users/Login");
            }

            string albumId = httpRequest.QueryData["albumId"].ToString();

            using (var context = new RunesDbContext())
            {
                Album albumFromDb = context.Albums
                    .SingleOrDefault(album => album.Id.ToString() == albumId);
                if (albumFromDb == null)
                {
                    return this.Redirect("/Albums/All");
                }
                string name = ((ISet<string>)httpRequest.FormData["name"]).FirstOrDefault();
                string link = ((ISet<string>)httpRequest.FormData["link"]).FirstOrDefault();
                string price = ((ISet<string>)httpRequest.FormData["price"]).FirstOrDefault();

                Track track = new Track
                {
                    Name = name,
                    Link = link,
                    Price = decimal.Parse(price)
                };
                albumFromDb.Tracks.Add(track);
                albumFromDb.Price = (albumFromDb.Tracks
                    .Select(t => t.Price)
                    .Sum() * 87) / 100;
                context.Update(albumFromDb);
                context.SaveChanges();
            }

            return this.Redirect($"/Albums/Details?id={albumId}");
        }

        public IHttpResponse Details(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                this.Redirect("/Users/Login");
            }
            var albumId = httpRequest.QueryData["albumId"].ToString();
            var trackId = httpRequest.QueryData["trackId"].ToString();

            using (var context = new RunesDbContext())
            {
                Track trackFromDb = context.Tracks.SingleOrDefault(track => track.Id.ToString() == trackId);

                if (trackFromDb == null)
                {
                    return this.Redirect($"/Albums/Details?id={albumId}");
                }
                this.ViewData["AlbumId"] = albumId;
                this.ViewData["Track"] = trackFromDb.ToHtmlDetails();
                return this.View();
            }
        }
    }
}