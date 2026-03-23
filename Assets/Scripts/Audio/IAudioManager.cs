using UnityEngine;

namespace Audio
{
    public interface IAudioManager
    {
        public void PlaySound(AudioClip clip);
        
        public void StopSound();
        
        public void PauseSound();
        
        public void ResumeSound();
        
        
    }
}