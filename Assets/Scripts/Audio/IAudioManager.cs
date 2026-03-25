using UnityEngine;

namespace Audio
{
    public interface IAudioManager
    {
        public void PlaySound(AudioClip clip, AudioSource source);
        
        public void StopSound(AudioSource source);
        
        public void PauseSound(AudioSource source);
        
        public void ResumeSound(AudioSource source);

        

    }
}