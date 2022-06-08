using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using BLL.APIHandler;
using BLL.Credentials;
using BLL.metaDataHelper;
using BLL.Model;
using BLL.Network;
using BLL.Player;
using Ookii.Dialogs.Wpf;
using UI.Inf;
using UI.Model;
using Timer = System.Timers.Timer;

namespace UI.ViewModel
{
    public class MainViewModel : NotifyPropertyChanged
    {
        #region ApiFields

        private OAuthTokenModel oAuth;

        private int _selectedTab;

        private ArtistManager artistManager;
        private AlbumManager albumManager;
        private TrackManager trackManager;
        private CredentialManger credentialManger;
        private YoutubeManager _youtubeManager;
        private TagLibManager tagLibManager;

        private string DownloadLocation = AppDomain.CurrentDomain.BaseDirectory;

        private ObservableCollection<ArtistApi> _artistApis;
        private ObservableCollection<AlbumApi> _albumApis;
        private ObservableCollection<TrackApi> _trackApis;

        private string _searchAlbum;
        private string _searchArtist;
        private string _searchTrack;

        private ICommand _SearchAlbumCommand;
        private ICommand _searchArtistCommand;
        private ICommand _searchTrackCommand;

        private ICommand _searchAlbumByArtistByIdCommand;
        private ICommand _searchTrackByAlbumCommand;

        private ICommand _selectDownloadLocation;
        private ICommand _downloadTrackCommand;

        #endregion ApiFields

        #region PlayerFields

        private PlayerManager playerManager;
        private bool isPaused = true;
        private TimeSpan _currentTimeSpan;
        private long _CurrentLenght;
        private Timer _timer;
        private int _volume;
        private bool isEnd = true;

        private ICommand shuffleCommand;
        private ICommand playFromWebCommand;
        private ICommand stopPlayCommand;
        private ICommand pauseCommand;
        private ICommand nextSongCommand;
        private ICommand previousSongCommand;

        private ThumbNailSong thumbNailSong;

        public TimeSpan CurrenTimeSpan
        {
            get => _currentTimeSpan;
            set
            {
                _currentTimeSpan = value;
                NotifyOfPropertyChanged();
            }
        }

        public int Volume
        {
            get => _volume;
            set
            {
                if (_volume != value)
                {
                    _volume = value;
                    playerManager.VolumeChange(Volume);
                }
            }
        }

        public long CurrentLenght
        {
            get => _CurrentLenght;
            set
            {
                if (thumbNailSong != null)
                {
                    _CurrentLenght = value;
                    if (playerManager != default)
                    {
                        playerManager.SetLenght(value);
                    }
                }
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _CurrentLenght = playerManager.GetPosition();
            NotifyOfPropertyChanged("CurrentLenght");
            CurrenTimeSpan = playerManager.GetCurrentTimeSpan();

            if (thumbNailSong != null)
            {
                if (playerManager.GetPosition() >= ThumbNailSong.length && isEnd)
                {
                    isEnd = false;
                    var index = 0;
                    try
                    {
                        index = TrackApis.IndexOf(TrackApis.Where(x => x.id == ThumbNailSong.id).First());
                    }
                    catch (Exception p)
                    {
                    }

                    playerManager.Pause();
                    playNext(index);
                }
            }
        }

        public ThumbNailSong ThumbNailSong
        {
            get => thumbNailSong;
            set
            {
                thumbNailSong = value;
                NotifyOfPropertyChanged();
            }
        }

        #endregion PlayerFields

        #region Commands and Collection Api

        public int SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                NotifyOfPropertyChanged();
            }
        }

        public string SearchTrack
        {
            get => _searchTrack;
            set
            {
                _searchTrack = value;
                NotifyOfPropertyChanged();
            }
        }

        public string SearchArtist
        {
            get => _searchArtist;
            set
            {
                _searchArtist = value;
                NotifyOfPropertyChanged();
            }
        }

        public string SearchAlbum
        {
            get => _searchAlbum;
            set
            {
                _searchAlbum = value;
                NotifyOfPropertyChanged();
            }
        }

        public ObservableCollection<AlbumApi> AlbumApis
        {
            get => _albumApis;
            set
            {
                _albumApis = value;
                NotifyOfPropertyChanged();
            }
        }

        public ObservableCollection<ArtistApi> ArtistApis
        {
            get => _artistApis;
            set
            {
                _artistApis = value;
                NotifyOfPropertyChanged();
            }
        }

        public ObservableCollection<TrackApi> TrackApis
        {
            get => _trackApis;
            set
            {
                _trackApis = value;
                NotifyOfPropertyChanged();
            }
        }

        public ICommand SearchAlbumCommand
        {
            get
            {
                if (_SearchAlbumCommand == null)
                {
                    _SearchAlbumCommand = new RelayCommand(param =>
                    {
                        Task.Run(() => AlbumApis = new ObservableCollection<AlbumApi>(albumManager.Search(SearchAlbum)));
                    });
                }

                return _SearchAlbumCommand;
            }
        }

        public ICommand SearchArtistCommand
        {
            get
            {
                if (_searchArtistCommand == null)
                {
                    _searchArtistCommand = new RelayCommand(param =>
                    {
                        Task.Run(() => ArtistApis = new ObservableCollection<ArtistApi>(artistManager.Search(SearchArtist)));
                    });
                }

                return _searchArtistCommand;
            }
        }

        public ICommand SearchTrackCommand
        {
            get
            {
                if (_searchTrackCommand == null)
                {
                    _searchTrackCommand = new RelayCommand(param =>
                    {
                        Task.Run(() => TrackApis = new ObservableCollection<TrackApi>(trackManager.Search(SearchTrack)));
                    });
                }

                return _searchTrackCommand;
            }
        }

        public ICommand SearchAlbumByArtistCommand
        {
            get
            {
                if (_searchAlbumByArtistByIdCommand == null)
                {
                    _searchAlbumByArtistByIdCommand = new RelayCommand(param =>
                    {
                        SelectedTab = 1;

                        Task.Run(() =>
                            AlbumApis = new ObservableCollection<AlbumApi>(
                                albumManager.SearchByArtist(param.ToString())));
                    });
                }

                return _searchAlbumByArtistByIdCommand;
            }
        }

        public ICommand SearchTrackByAlbumCommand
        {
            get
            {
                if (_searchTrackByAlbumCommand == null)
                {
                    _searchTrackByAlbumCommand = new RelayCommand(param =>
                    {
                        SelectedTab = 2;

                        Task.Run(() =>
                            TrackApis = new ObservableCollection<TrackApi>(
                                trackManager.SearchByAlbum(albumManager.getAlbumById(param.ToString()))));
                    });
                }
                return _searchTrackByAlbumCommand;
            }
        }

        public ICommand DownloadTrackCommand
        {
            get
            {
                if (_downloadTrackCommand == null)
                {
                    _downloadTrackCommand = new RelayCommand(param =>
                    {
                        var track = trackManager.getTrackById(param.ToString());
                        var Selectedid = param.ToString();
                        Task.Run(() =>
                        {
                            var asd = TrackApis.Where(x => x.id == Selectedid).First();
                            asd.Status = "Downloading";
                            TrackApis = new ObservableCollection<TrackApi>(TrackApis);

                            var path = _youtubeManager.DownloadFromYoutubeAudioTaskWithSearch(
                                track.name + " " + track.AllArtists.Trim(), DownloadLocation);
                            path.Wait();
                            /*tagLibManager.SetMetaDataTrackApi(path.Result, track);*/
                            asd.Status = "Finished";
                            TrackApis = new ObservableCollection<TrackApi>(TrackApis);
                        });
                    });
                }

                return _downloadTrackCommand;
            }
        }

        public ICommand SelectDownloadLocation
        {
            get
            {
                if (_selectDownloadLocation == null)
                {
                    _selectDownloadLocation = new RelayCommand(param =>
                    {
                        var dialog = new VistaFolderBrowserDialog();

                        if (dialog.ShowDialog() == true)
                        {
                            DownloadLocation = dialog.SelectedPath;
                        }
                    }, null);
                }

                return _selectDownloadLocation;
            }
        }

        #endregion Commands and Collection Api

        #region PlayerCommands

        public ICommand ShuffleCommand
        {
            get
            {
                if (shuffleCommand == null)
                {
                    shuffleCommand = new RelayCommand(param =>
                    {
                        var i = 0;
                        var tmplist = TrackApis;
                        while (i < TrackApis.Count)
                        {
                            TrackApis.Move(0, new Random().Next(0, TrackApis.Count - 1));
                            i++;
                        }

                        NotifyOfPropertyChanged("TrackApis");
                    });
                }

                return shuffleCommand;
            }
        }

        public ICommand PlayFromWebCommand
        {
            get
            {
                if (playFromWebCommand == null)
                {
                    playFromWebCommand = new RelayCommand(param =>
                    {
                        var id = param.ToString();
                        Thread.Sleep(200);
                        Task.Run((() =>
                        {
                            playSongFromWeb(id);
                        }));

                        isPaused = false;
                    });
                }

                return playFromWebCommand;
            }
        }

        public ICommand NextSongCommand
        {
            get
            {
                if (nextSongCommand == null)
                {
                    nextSongCommand = new RelayCommand(param =>
                    {
                        playerManager.Pause();
                        if (ThumbNailSong != null)
                        {
                            int index = 0;
                            try
                            {
                                index = TrackApis.IndexOf(TrackApis.Where(x => x.id == ThumbNailSong.id).First());
                            }
                            catch (Exception e)
                            {
                            }
                            playNext(index);
                        }
                    });
                }

                return nextSongCommand;
            }
        }

        public ICommand PauseCommand
        {
            get
            {
                if (pauseCommand == null)
                {
                    pauseCommand = new RelayCommand(param =>
                    {
                        if (isPaused)
                        {
                            playerManager.Play();
                            isPaused = false;
                        }
                        else
                        {
                            isPaused = true;
                            playerManager.Pause();
                        }
                    });
                }

                return pauseCommand;
            }
        }

        public ICommand PreviousSongCommand
        {
            get
            {
                if (previousSongCommand == null)
                {
                    previousSongCommand = new RelayCommand(param =>
                    {
                        if (ThumbNailSong != null)
                        {
                            playerManager.Pause();

                            int index = 0;
                            try
                            {
                                index = TrackApis.IndexOf(TrackApis.Where(x => x.id == ThumbNailSong.id).First());
                            }
                            catch (Exception e)
                            {
                            }

                            if (index > 0)
                            {
                                index--;
                            }
                            else
                            {
                                index = TrackApis.Count - 1;
                            }

                            Task.Run((() =>
                            {
                                playSongFromWeb(TrackApis[index].id);
                            }));
                        }
                    });
                }

                return previousSongCommand;
            }
        }

        #endregion PlayerCommands

        #region PlayerFunction

        public void playNext(int index)
        {
            if (index < TrackApis.Count - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }

            Task.Run((() =>
            {
                playSongFromWeb(TrackApis[index].id);
            }));
        }

        public void playSongFromWeb(string IdOfSong)
        {
            playerManager.Reuse(_youtubeManager.getUrlForPlayer(_trackApis.Where(x => x.id == IdOfSong).First().name + " "
                + _trackApis.Where(x => x.id == IdOfSong).First().AllArtists).Result);
            ThumbNailSong = new ThumbNailSong(_trackApis.Where(x => x.id == IdOfSong).First(), playerManager.GetLenght());
            playerManager.Play();
            isEnd = true;
        }

        #endregion PlayerFunction

        public MainViewModel(CredentialManger credentialManger,
            YoutubeManager youtubeManager, PlayerManager playerManager)
        {
            this.credentialManger = credentialManger;
            oAuth = credentialManger.getToken();
            albumManager = new AlbumManager(oAuth);
            trackManager = new TrackManager(oAuth);
            artistManager = new ArtistManager(oAuth);
            this._youtubeManager = youtubeManager;
            this.playerManager = playerManager;
            AlbumApis = new ObservableCollection<AlbumApi>(albumManager.Search());
            ArtistApis = new ObservableCollection<ArtistApi>(artistManager.Search());
            TrackApis = new ObservableCollection<TrackApi>(trackManager.Search());
            _timer = new Timer();
            _timer.Interval = 50;
            _timer.Elapsed += Timer_Elapsed;
            Task.Run((() =>
            {
                _timer.Start();
            }));
        }
    }
}