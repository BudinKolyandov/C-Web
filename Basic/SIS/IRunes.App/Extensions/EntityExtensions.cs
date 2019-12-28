﻿using IRunes.Models;
using System;
using System.Linq;
using System.Net;

namespace IRunes.App.Extensions
{
    public static class EntityExtensions
    {
        private static string GetTracks(Album album)
        {
            return 
                album.Tracks.Count == 0
                ? "There are no tracks in this album!"
                :string.Join("", album.Tracks
                .Select((track, indexer) => track.ToHtmlAll(album.Id,indexer + 1)));
        }

        public static string ToHtmlAll(this Album album)
        {
            return $"<div><a href=\"/Albums/Details?id={album.Id}\">{WebUtility.UrlDecode(album.Name)}</a></div>";
        }

        public static string ToHtmlDetails(this Album album)
        {
            return "<div class=\"album-details\">" +
                   "    <div class=\"album-data\">" +
                  $"        <img src=\"{WebUtility.UrlDecode(album.Cover)}\"><br />" +
                  $"        <h1>Album Name: {WebUtility.UrlDecode(album.Name)}</h1>"+
                  $"        <h1>Album Price: {album.Price:f2}</h1>" +
                  $"        <br />" +
                   "    </div>" +
                   "    <div class=\"album-tracks\">" +
                   "    <h1>Tracks</h1>" +  
                   "    <hr style\"height: 2px\">" +
                  $"    <a href=\"/Tracks/Create?albumId={album.Id}\">Create Track</a>" +
                   "    <hr style\"height: 2px\">" +
                   "    <ul class=\"tracks-list\">" +
                  $"        {GetTracks(album)}"+ 
                   "    </ul>" +
                   "    <hr style\"height: 2px\">" +
                   "    </div>" +
                   "<hr style\"height: 2px\">" +
                   "</div>";
        }

        public static string ToHtmlAll(this Track track, Guid albumId, int index)
        {
            return $"<li><strong>{index}</strong>. <a href=\"/Tracks/Detaisl?albumId={albumId}&trackId={track.Id}\">{WebUtility.UrlDecode(track.Name)}</a></li>";
        }

        public static string ToHtmlDetails(this Track track)
        {
            return "<div class=\"track-details\">" +
                   $"    <iframe src=\"{WebUtility.UrlDecode(track.Link)}\" width=\"640\" height=\"480\"></iframe>" +
                   $"    <h1>Track Name: {WebUtility.UrlDecode(track.Name)}</h1>" +
                   $"    <h1>Track Price: {track.Price:f2}</h1>" +
                   "</div>";
        }

    }
}