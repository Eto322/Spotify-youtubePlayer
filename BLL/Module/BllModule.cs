using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.APIHandler;
using BLL.Credentials;
using BLL.metaDataHelper;
using BLL.Model;
using BLL.Network;
using BLL.Player;
using Ninject.Modules;

namespace BLL.Module
{
    public class BllModule:NinjectModule

    {
    


         public override void Load()
        {
            Bind<CredentialManger>().To<CredentialManger>();
            Bind<OAuthTokenModel>().To<OAuthTokenModel>();
            Bind<AlbumManager>().To<AlbumManager>();
            Bind<TrackManager>().To<TrackManager>();
            Bind<ArtistManager>().To<ArtistManager>();
            Bind<YoutubeManager>().To<YoutubeManager>();
            Bind<TagLibManager>().To<TagLibManager>();
            Bind<PlayerManager>().To<PlayerManager>();



        }
    }
}
