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
    public class AlbumManager
    {

        private NetworkManager manager = new NetworkManager();
        private OAuthTokenModel token;


        public AlbumManager(OAuthTokenModel token)
        {
            this.token = token;

        }

        public AlbumApi getAlbumById(string Id)
        {
            var json = manager.GetJsonSpotify($"https://api.spotify.com/v1/albums/{Id}", token);

            AlbumApi result = JsonConvert.DeserializeObject<AlbumApi>(json);
            foreach (var artist in result.artists)
            {
                result.AllArtists += artist.name + "\n";
            }

            result.AlbumCover = result.images[1].url;
            return result;
        }


        public List<AlbumApi> Search(string searchName=null, int limit = 10, int offset = 0)
        {
            if (string.IsNullOrEmpty(searchName))
            {
                searchName = "Siren Charm";
            }


            searchName = Regex.Replace(searchName, @"\s+", "%20");
            var json = manager.GetJsonSpotify(
                $"https://api.spotify.com/v1/search?q={searchName}&type=album&limit={limit}&offset={offset}", token);
            JObject search = JObject.Parse(json);
            IList<JToken> results = search["albums"]["items"].Children().ToList();
            var returnAlbums = new List<AlbumApi>();


            foreach (JToken result in results)
            {
                AlbumApi album = JsonConvert.DeserializeObject<AlbumApi>(result.ToString());
                foreach (var artist in album.artists)
                {
                    album.AllArtists += artist.name + "\n";
                }

                album.AlbumCover = album.images[1].url;
                returnAlbums.Add(album);
            }


            return returnAlbums;

        }

        public List<AlbumApi> SearchByArtist(string id, int limit = 50, int offset = 0)
        {

            var json = manager.GetJsonSpotify(
                $"https://api.spotify.com/v1/artists/{id}/albums?%2Cappears_on&limit={limit}&offset={offset}", token);
            JObject search = JObject.Parse(json);
            IList<JToken> results = search["items"].Children().ToList();
            var returnAlbums = new List<AlbumApi>();


            foreach (JToken result in results)
            {
                AlbumApi album = JsonConvert.DeserializeObject<AlbumApi>(result.ToString());
                foreach (var artist in album.artists)
                {
                    album.AllArtists += artist.name + "\n";
                }

                album.AlbumCover = album.images[1].url;
                returnAlbums.Add(album);
            }


            return returnAlbums;

        }


    }
}
