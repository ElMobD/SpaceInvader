using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SpaceInvaders
{
    class AudioSfx
    {
        static List<IWavePlayer> waveOutEvents = new List<IWavePlayer>();
        public AudioSfx() {}
        
        static string GetPath(UnmanagedMemoryStream theMusic)
        {
            if (theMusic != null && theMusic.Length > 0)
            {
                string tempFilePath = Path.GetTempFileName();

                using (FileStream fileStream = File.Create(tempFilePath))
                {
                    theMusic.CopyTo(fileStream);
                }

                return tempFilePath;
            }
            return string.Empty;
        }
        internal static void PlaySound(UnmanagedMemoryStream theMusic)
        {
            
            string soundFilePath = GetPath(theMusic);
            var waveOutEvent = new WaveOut();
            var audioFileReader = new AudioFileReader(soundFilePath);
            waveOutEvent.Init(audioFileReader);
            waveOutEvent.PlaybackStopped += (sender, args) =>
            {
                if (waveOutEvents.Contains((IWavePlayer)sender)){
                    waveOutEvents.Remove((IWavePlayer)sender);
                    ((IDisposable)sender).Dispose(); 
                }
            };
            waveOutEvents.Add(waveOutEvent);
            waveOutEvent.Play();
        }
    }
}
