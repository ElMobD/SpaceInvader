using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Media;

namespace SpaceInvaders
{
    class Audio : IDisposable
    {
        private SoundPlayer soundPlayer;
        private bool played;
        

        public Audio(UnmanagedMemoryStream audioData)
        {
            soundPlayer = new SoundPlayer(audioData);
            played = false;
        }
        public void Play()
        {
            if (soundPlayer != null)
            {
                soundPlayer.Play();
                played = true;
            }
        }
        public void Stop()
        {
            if (soundPlayer != null)
            {
                soundPlayer.Stop();
                played = false;
            }
        }
        public void Dispose()
        {
            if (soundPlayer != null)
            {
                soundPlayer.Dispose();
                soundPlayer = null;
                played = false;
            }
        }
        public bool IsPlayed()
        {
            return played;
        }
    }
}
