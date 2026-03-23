using Audio;
using UnityEngine;
using Zenject;

namespace Core
{
    [CreateAssetMenu(fileName = "AudioInstaller", menuName = "Installers/AudioInstaller")]
    public class AudioInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAudioManager>().To<AudioManager>().AsSingle();
        }
    }
}