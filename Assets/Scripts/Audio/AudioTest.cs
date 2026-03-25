using UnityEngine;
using Zenject;

namespace Audio
{
    public class AudioTest : MonoBehaviour
    {
        public AudioClip clip;
        
        private IAudioManager _audioManager;
        private AudioSource _audioSource;

        [Inject]
        public void Construct(IAudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        void Start()
        {
            Debug.Log($"AudioManager null: {_audioManager == null}");
            _audioSource = gameObject.AddComponent<AudioSource>();
    
            if (_audioManager == null)
            {
                Debug.LogError("Zenject no inyectó. ¿Arrancaste desde Bootstrap?");
                return;
            }
    
            _audioManager.PlaySound(clip, _audioSource);
        }
    }
}