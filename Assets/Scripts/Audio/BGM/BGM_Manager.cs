using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Audio.BGM
{
    public class BGM_Manager : MonoBehaviour
    {
        [Header("BGM Settings")]
        public BGMEntry[] entries;
        public string bootstrapSceneName = "Bootstrap";
        public float fadeInDuration = 2f;

        private IAudioManager _audioManager;
        private AudioSource _audioSource;

        void Awake()
        {
            DontDestroyOnLoad(this);
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        [Inject]
        public void Construct(IAudioManager audioManager)
        {
            _audioManager = audioManager;
    
          
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.loop = true;
            }
    
            PlayBGMForScene(SceneManager.GetActiveScene());
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        private void OnSceneChanged(Scene previousScene, Scene newScene)
        {
            if (_audioManager == null) return;
            if (newScene.name != bootstrapSceneName)
                PlayBGMForScene(newScene);
        }

        private void PlayBGMForScene(Scene scene)
        {
            var entry = Array.Find(entries, e => e.sceneName == scene.name);
            if (entry != null)
                StartCoroutine(FadeIn(entry.clip, _audioSource, fadeInDuration));
        }

        private IEnumerator FadeIn(AudioClip clip, AudioSource source, float duration)
        {
            source.Stop();
            source.clip = clip;
            source.volume = 0f;
            source.Play();

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                source.volume = Mathf.Clamp01(elapsed / duration);
                yield return null;
            }

            source.volume = 1f;
        }
    }
}