using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BLL.Model;
using BLL.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BLL.APIHandler
{
    public class TrackManager
    {
        private NetworkManager network;
        private OAuthTokenModel token;

        public TrackManager(OAuthTokenModel token)
        {
            this.network = new NetworkManager();
            this.token = token;
        }

        public TrackApi getTrackById(string Id)
        {
            var json = network.GetJsonSpotify($"https://api.spotify.com/v1/tracks/{Id}", token);

            TrackApi result = JsonConvert.DeserializeObject<TrackApi>(json);
            result.ActualDuration = string.Format("{0}:{1}", TimeSpan.FromMilliseconds(result.duration_ms).Minutes, TimeSpan.FromMilliseconds(result.duration_ms).Seconds);
            result.Album = result.album.name;
            foreach (var res in result.artists)
            {
                result.AllArtists += res.name + "\n";
            }
            result.TrackCover = result.album.images[1].url;

            return result;
        }

        public List<TrackApi> Search(string searchName=null, int limit = 10, int offset = 0)
        {
            if (string.IsNullOrEmpty(searchName))
            {
                searchName = "Little lion Man";
            }

            searchName = Regex.Replace(searchName, @"\s+", "%20");
            var json = network.GetJsonSpotify(
                $"https://api.spotify.com/v1/search?q={searchName}&type=track&limit={limit}&offset={offset}", token);
            JObject search = JObject.Parse(json);
            IList<JToken> results = search["tracks"]["items"].Children().ToList();
            var returnTracks = new List<TrackApi>();

            foreach (var result in results)
            {
                var track = JsonConvert.DeserializeObject<TrackApi>(result.ToString());

                track.Album = track.album.name;
                foreach (var res in track.artists)
                {
                    track.AllArtists += res.name + "\n";
                }
                track.TrackCover = track.album.images[1].url;
                track.ActualDuration = string.Format("{0}:{1}", TimeSpan.FromMilliseconds(track.duration_ms).Minutes, TimeSpan.FromMilliseconds(track.duration_ms).Seconds);

                returnTracks.Add(track);

            }

            return returnTracks;
        }

        public List<TrackApi> SearchByAlbum(AlbumApi album, int limit = 50, int offset = 0)
        {
            var json = network.GetJsonSpotify(
                $"https://api.spotify.com/v1/albums/{album.id}/tracks?limit={limit}&offset={offset}", token);
            JObject search = JObject.Parse(json);
            
            IList<JToken> results = search["items"].Children().ToList();
            var returnTracks = new List<TrackApi>();

            foreach (var result in results)
            {
                var track = JsonConvert.DeserializeObject<TrackApi>(result.ToString());

                track.Album = album.name;
                foreach (var res in track.artists)
                {
                    track.AllArtists += res.name + "\n";
                }

                track.TrackCover = album.images[1].url;
                track.ActualDuration = string.Format("{0}:{1}", TimeSpan.FromMilliseconds(track.duration_ms).Minutes, TimeSpan.FromMilliseconds(track.duration_ms).Seconds);

                returnTracks.Add(track);

            }

            return returnTracks;
        }
    }
}
