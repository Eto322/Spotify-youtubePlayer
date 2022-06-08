using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace BLL.Player
{
    public class PlayerManager
    {
        private MediaFoundationReader Player;
        private WaveOutEvent WaveOut;


        public PlayerManager()
        {
            
        }
        public PlayerManager(string path)
        {
            Player = new MediaFoundationReader(path);
            WaveOut = new WaveOutEvent();
            WaveOut.Init(Player);
        }

        public void Play()
        {
            if (Player!=null)
            {
                WaveOut.Play();
            }
            
        }

        public void Stop()
        {
            WaveOut.Stop();
        }

        public bool isPlaying()
        {
            switch (WaveOut.PlaybackState)
            {
                case PlaybackState.Playing:
                    return true;
                    break;
                case PlaybackState.Paused:
                    return false;
                    break;
            }

            return false;

        }
        public void Pause()
        {

            if (Player!=null)
            {
                WaveOut.Pause();
            }
            
        }

        public TimeSpan GetCurrentTimeSpan()
        {
            if (Player==null) 
            {
                return TimeSpan.Zero;
            }
            return Player.CurrentTime;
        }

        public void VolumeChange(int volume)
        {
            WaveOut.Volume = volume/100f;
        }

        public float getVolume()
        {
            return WaveOut.Volume;
        }

        public TimeSpan GetTotalTimeSpan()
        {
            
            return Player.TotalTime;
        }

        public void SetLenght(long lenght)
        {
            Player.Position=lenght;
        }

        public long GetLenght()
        {
            if (Player==null)
            {
                return -1;
            }
            return Player.Length;
        }

        public long GetPosition()
        {
            if (Player==null)
            {
                return -1;
            }
            return Player.Position;
        }

        public void Reuse(string path)
        {
            if (Player==null)
            {
                Player = new MediaFoundationReader(path);
                WaveOut = new WaveOutEvent();
                WaveOut.Init(Player);
                return;
            }

            WaveOut.Dispose();
            Player.Dispose();
            Player = new MediaFoundationReader(path);
            WaveOut = new WaveOutEvent();
            WaveOut.Init(Player);

        }

        


    }
}
