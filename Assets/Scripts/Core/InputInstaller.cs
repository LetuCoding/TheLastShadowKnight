using Audio;
using UnityEngine;
using Zenject;

namespace Core
{
    [CreateAssetMenu(fileName = "InputInstaller", menuName = "Installers/InputInstaller")]
    public class InputInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InputSystem_Actions>().AsSingle();
        }
    }
}