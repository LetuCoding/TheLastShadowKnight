using System;
using System.Collections;
using UnityEngine;

namespace Audio
{
    public class AudioManager :  IAudioManager
    {
      
        
        public void PlaySound(AudioClip clip, AudioSource source)
        {
            source.clip = clip;
            source.Play();
            
        }
        
        
        public void StopSound(AudioSource source)
        {
            source.Stop();
        }

        public void PauseSound(AudioSource source)
        {
            source.Pause();
        }

        public void ResumeSound(AudioSource source)
        {
            source.UnPause();
        }

        public void PlaySoundWithFadeIn()
        {
            throw new NotImplementedException();
        }
    }
}