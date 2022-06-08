using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BLL.Model;
using TagLib;

namespace BLL.metaDataHelper
{
    public class TagLibManager
    {
        public void SetMetaDataTrackApi(string path, TrackApi track)
        {
            var file = TagLib.File.Create(path);
            file.Tag.Title = track.name;
            file.Tag.AlbumArtists = new[] { track.AllArtists };
            file.Tag.Album = track.Album;

            string pathPicture = string.Format(@"D:\temp\{0}.jpg", Guid.NewGuid().ToString());
            byte[] imageBytes;
            using (WebClient client = new WebClient())
            {
                imageBytes = client.DownloadData(new Uri(track.TrackCover));
            }

            file.Tag.Pictures = new IPicture[] { new Picture(imageBytes) };

            file.Save();
            
        }

       
    }
}
