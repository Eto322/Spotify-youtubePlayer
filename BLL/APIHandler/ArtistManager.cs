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
    public class ArtistManager
    {
        private NetworkManager manager = new NetworkManager();
        private OAuthTokenModel token;
        public ArtistManager(OAuthTokenModel token)
        {
            this.token = token;
        }

        public ArtistApi getArtistById(string Id)
        {
            var json = manager.GetJsonSpotify($"https://api.spotify.com/v1/artists/{Id}", token);

            ArtistApi result = JsonConvert.DeserializeObject<ArtistApi>(json);
            result.ArtistCover = result.images[1].url;
            return result;
        }

        public List<ArtistApi> Search(string searchName=null, int limit = 10, int offset = 0)
        {
            if (string.IsNullOrEmpty(searchName))
            {
                searchName = "MaybeSheWill";
            }

            searchName = Regex.Replace(searchName, @"\s+", "%20");
            var json = manager.GetJsonSpotify($"https://api.spotify.com/v1/search?q={searchName}&type=artist&limit={limit}&offset={offset}", token);
            JObject search = JObject.Parse(json);
            IList<JToken> results = search["artists"]["items"].Children().ToList();
           



            List<ArtistApi> returnresult = new List<ArtistApi>();


            foreach (JToken result in results)
            {
                ArtistApi artist = JsonConvert.DeserializeObject<ArtistApi>(result.ToString());

                if (artist.images.Count>0)
                {
                    artist.ArtistCover = artist.images[1].url;
                }
                
                returnresult.Add(artist);
            }

            

            return returnresult;
        }
        


    }
}
