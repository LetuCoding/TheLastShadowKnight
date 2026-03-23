using Audio;
using UnityEngine;
using Zenject;

namespace Core
{
    public class AudioInstaller : MonoInstaller
    {
        
        [SerializeField] private AudioManager audioManager;
        
        public override void InstallBindings()
        {
            Container.Bind<IAudioManager>().FromInstance(audioManager).AsSingle();
        }
    }
}