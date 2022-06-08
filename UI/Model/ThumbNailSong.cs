using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Model;

namespace UI.Model
{
    public class ThumbNailSong
    {
        public string id;
        public string name { get; set; }
        public string artist { get; set; }
        public string ImageUri { get; set; }
        public string ThumbnailString { get; set; }

        public long length { get; set; }

        public ThumbNailSong(string name, string artist, string imageUri)
        {
            id = string.Empty;
            this.name = name;
            this.artist = artist;
            this.ImageUri = imageUri;
            this.ThumbnailString = name + "\n" + artist;
        }

        public ThumbNailSong(TrackApi track, long length)
        {
            this.id = track.id;
            this.name = track.name;
            this.artist = track.AllArtists;
            ImageUri = track.TrackCover;
            this.ThumbnailString = name + "\n" + artist;
            this.length = length;
        }
    }
}