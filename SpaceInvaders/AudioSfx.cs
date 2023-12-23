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
                    ((IDisposable)sender).Dispose(); // Libérez les ressources associées
                }
            };
            waveOutEvents.Add(waveOutEvent);
            waveOutEvent.Play();
        }


        /*internal static void PlaySound(string soundFilePath)
        {
            // Créez une nouvelle instance de WaveOut
            var waveOutEvent = new WaveOut();

            // Chargez le fichier audio
            var audioFileReader = new AudioFileReader(soundFilePath);

            // Attachez l'AudioFileReader à WaveOut
            waveOutEvent.Init(audioFileReader);

            // Écoutez l'événement PlaybackStopped pour libérer les ressources après la lecture
            waveOutEvent.PlaybackStopped += (sender, args) =>
            {
                if (waveOutEvents.Contains((IWavePlayer)sender))
                {
                    waveOutEvents.Remove((IWavePlayer)sender);
                    ((IDisposable)sender).Dispose(); // Libérez les ressources associées
                }
            };

            // Ajoutez la nouvelle instance à la liste
            waveOutEvents.Add(waveOutEvent);

            // Jouez le son
            waveOutEvent.Play();
        }*/
        private void GetFile()
        {

        }
    }
}
