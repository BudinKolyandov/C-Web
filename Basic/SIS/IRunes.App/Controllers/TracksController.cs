﻿using IRunes.App.Extensions;
using IRunes.App.ViewModels;
using IRunes.Models;
using IRunes.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IRunes.App.Controllers
{
    public class TracksController : Controller
    {
        private readonly ITrackService trackService;

        private readonly IAlbumService albumService;

        public TracksController()
        {
            this.trackService = new TrackService();
            this.albumService = new AlbumService();
        }


        [Authorize]
        public ActionResult Create()
        {
            string albumId = this.Request.QueryData["albumId"].ToString();

            return this.View(new TrackCreateViewModel { Id = albumId} );
        }

        [Authorize]
        [HttpPost(ActionName = "Create")]
        public ActionResult CreateConfirm()
        {
            string albumId = this.Request.QueryData["albumId"].ToString();

            string name = ((ISet<string>)this.Request.FormData["name"]).FirstOrDefault();
            string link = ((ISet<string>)this.Request.FormData["link"]).FirstOrDefault();
            string price = ((ISet<string>)this.Request.FormData["price"]).FirstOrDefault();

            Track track = new Track
            {
                Id = new Guid().ToString(),
                Name = name,
                Link = link,
                Price = decimal.Parse(price)
            };

            if (!this.albumService.AddTrackToAlbum(albumId, track))
            {
                return this.Redirect($"/Albums/All");
            }

            return this.Redirect($"/Albums/Details?id={albumId}");
        }

        [Authorize]
        public ActionResult Details()
        {
            var albumId = this.Request.QueryData["albumId"].ToString();
            var trackId = this.Request.QueryData["trackId"].ToString();

            Track trackFromDb = this.trackService.GetTrackById(trackId);

            if (trackFromDb == null)
            {
                return this.Redirect($"/Albums/Details?id={albumId}");
            }
            //this.ViewData["AlbumId"] = albumId;
            //this.ViewData["Track"] = trackFromDb.ToHtmlDetails(albumId);
            return this.View();

        }
    }
}
