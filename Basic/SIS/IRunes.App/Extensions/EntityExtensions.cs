using IRunes.Models;
using System.Linq;

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
                .Select((track, indexer) => track.ToHtmlAll(indexer + 1)));
        }

        public static string ToHtmlAll(this Album album)
        {
            return $"<div><a href=\"/Albums/Details?id={album.Id}\">{album.Name}</a></div>";
        }

        public static string ToHtmlDetails(this Album album)
        {
            return "<div class=\"album-details\">" +
                   "    <div class=\"album-data\">" +
                  $"        <img src=\"{album.Cover}\"><br />" +
                  $"        <h1>Album Name: {album.Name}</h1>"+
                  $"        <h1>Album Price: {album.Price:f2}</h1>" +
                  $"        <br />" +
                   "    </div>" +
                   "    <div class=\"album-tracks\">" +
                   "    <h1>Tracks</h1>" +  
                   "    <hr style\"height: 2px\">" +
                   "    <a href=\"/Tracks/Create\">Create Track</a>" +
                   "    <hr style\"height: 2px\">" +
                   "    <ul class=\"tracks-list\">" +
                  $"        {GetTracks(album)}"+ 
                   "    </ul>" +
                   "    <hr style\"height: 2px\">" +
                   "    </div>" +
                   "<hr style\"height: 2px\">" +
                   "</div>";
        }

        public static string ToHtmlAll(this Track track, int index)
        {
            return $"<li><a href=\"/Tracks/Detaisl?id={track.Id}\">{index}. {track.Name}</li>";
        }

        public static string ToHtmlDetails(this Track track)
        {
            return null;
        }

    }
}
