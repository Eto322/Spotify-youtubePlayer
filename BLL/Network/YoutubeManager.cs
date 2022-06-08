using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BLL.metaDataHelper;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace BLL.Network
{
    public class YoutubeManager

    {
        public async Task<string> DownloadFromYoutubeAudioTask(string url, string path)
        {
            var youtube = new YoutubeClient();
            var manifest = await youtube.Videos.Streams.GetManifestAsync(url);
            var streamInfo = manifest.GetAudioOnlyStreams().GetWithHighestBitrate();
            var stream = await youtube.Videos.Streams.GetAsync(streamInfo);
            await youtube.Videos.Streams.DownloadAsync(streamInfo, path + "\\song.mp3");

            return string.Empty;
        }

        public async Task<string> getUrlForPlayer(string search)
        {
            var youtube = new YoutubeClient();
            var videos = await youtube.Search.GetVideosAsync(search).CollectAsync(1);
            var manifest = await youtube.Videos.Streams.GetManifestAsync(videos[0].Id);
            var streamInfo = manifest.GetAudioOnlyStreams().GetWithHighestBitrate();
            return streamInfo.Url;
        }

        public async Task<string> DownloadFromYoutubeAudioTaskWithSearch(string search, string path)
        {
            var youtube = new YoutubeClient();
            var videos = await youtube.Search.GetVideosAsync(search).CollectAsync(1);
            var manifest = await youtube.Videos.Streams.GetManifestAsync(videos[0].Id);
            var streamInfo = manifest.GetAudioOnlyStreams().GetWithHighestBitrate();
            var stream = await youtube.Videos.Streams.GetAsync(streamInfo);
            var filename = path + $"\\{videos[0].Title}.mp3";
           

            await youtube.Videos.Streams.DownloadAsync(streamInfo, filename);

            




            return filename;
        }

        public async Task<string> SearchVideo(string search)
        {
            var youtube = new YoutubeClient();
            var videos = await youtube.Search.GetVideosAsync(search);

            return videos[0].Url;

        }

      

    }
}
