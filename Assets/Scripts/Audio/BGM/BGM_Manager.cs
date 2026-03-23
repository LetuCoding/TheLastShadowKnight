using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Audio
{
    public class BGM_Manager : MonoBehaviour
    {
        [Header("BGM Settings")]
        public BGMEntry[] entries;

        private IAudioManager _audioManager;
        private AudioSource _audioSource;

        void Awake()
        {
            DontDestroyOnLoad(this);
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = true;
        }

        private void Start()
        {
            PlayBGMForScene(SceneManager.GetActiveScene());
        }

        [Inject]
        public void Construct(IAudioManager audioManager)
        {
            _audioManager = audioManager;
            SceneManager.activeSceneChanged += OnSceneChanged;
        }
        

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        private void OnSceneChanged(Scene previousScene, Scene newScene)
        {
            PlayBGMForScene(newScene);
        }

        private void PlayBGMForScene(Scene scene)
        {
            var entry = Array.Find(entries, e => e.sceneName == scene.name);
            if (entry != null)
                _audioManager.PlaySound(entry.clip, _audioSource);
        }
    }
}