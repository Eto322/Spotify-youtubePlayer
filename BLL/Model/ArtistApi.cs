using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Model
{
    public class ArtistApi
    {
        public ExternalUrlsApi external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
        public List<ImageApi> images { get; set; }
        
        public string ArtistCover { get; set; }
    }
}
