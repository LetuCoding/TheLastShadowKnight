using System;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }

        public void StopSound()
        {
            audioSource.Stop();
        }

        public void PauseSound()
        {
            audioSource.Pause();
        }

        public void ResumeSound()
        {
            audioSource.UnPause();
        }

        
    }
}