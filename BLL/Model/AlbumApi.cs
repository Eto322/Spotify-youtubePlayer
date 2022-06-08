using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Model
{
    public class AlbumApi
    {
        public string album_type { get; set; }
        public List<ArtistApi> artists { get; set; }
        public ExternalUrlsApi external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<ImageApi> images { get; set; }
        public string name { get; set; }
        public string release_date { get; set; }
        public string release_date_precision { get; set; }
        public int total_tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }

        public string AllArtists { get; set; }
        public string AlbumCover { get; set; }
        
    }
}

