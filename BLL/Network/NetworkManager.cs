using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BLL.Model;

namespace BLL.Network
{
    public class NetworkManager
    {
        
            public string GetJsonSpotify(string url, OAuthTokenModel token)
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Accept = "application/json";
                webRequest.Headers.Add($"Authorization: Bearer { token.access_token}");
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                string text;
                using (var sr=new StreamReader(response.GetResponseStream()))
                {
                     text = sr.ReadToEnd();
                }

                return text;
            }
        
    }
}
