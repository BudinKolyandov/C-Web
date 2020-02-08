using IRunes.Models;
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
            return $"<h3><a href=\"/Albums/Details?id={album.Id}\">{WebUtility.UrlDecode(album.Name)}</a></h3>";
        }


        public static string ToHtmlDetails(this Album album)
        {
            return null;
        }

        public static string ToHtmlAll(this Track track, Guid albumId, int index)
        {
            return $"<li><strong>{index}</strong>. <a href=\"/Tracks/Details?albumId={albumId}&trackId={track.Id}\"><i>{WebUtility.UrlDecode(track.Name)}</i></a></li>";
        }

        public static string ToHtmlDetails(this Track track, string albumId)
        {
            return "<div class\"track-details\">" +
                  $"    <h1 class=\"text-center\">Track Name: {WebUtility.UrlDecode(track.Name)}</h1>" +
                  $"    <h1 class=\"text-center\">Track Price: {track.Price:F2}</h1>" +
                   "    <hr class=\"bg-success w-50\" style=\"height: 2px\" />" +
                   "    <div class=\"d-flex justify-content-center\">" +
                  $"          <iframe src=\"{WebUtility.UrlDecode(track.Link)}\" class=\"w-50\" height=\"480\"></iframe>" +
                   "    </div>" +
                   "    <hr class=\"bg-success w-50\" style=\"height: 2px\" />" +
                   "    <div class=\"d-flex justify-content-center\">" +
                  $"        <a href=\"/Albums/Details?id={albumId}\" class=\"btn bg-success text-white\">Back To Album</a>" +
                   "    </div>" +
                   "</div>";
        }

    }
}
