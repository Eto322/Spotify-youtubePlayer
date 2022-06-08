using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Model
{
    public class TrackApi
    {
        public AlbumApi album { get; set; }
        public List<ArtistApi> artists { get; set; }
        public int duration_ms { get; set; }
        public ExternalUrlsApi external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string preview_url { get; set; }
        public int track_number { get; set; }
        public string type { get; set; }
        public string uri { get; set; }

        public string Album { get; set; }
        public string AllArtists { get; set; }
        public string TrackCover { get; set; }
        public  string ActualDuration { get; set; }
        public string Status { get; set; } = " ";

    }
}
