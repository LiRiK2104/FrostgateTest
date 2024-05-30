using Core.Services;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class InputListenerInstaller : MonoInstaller
    {
        [SerializeField] private InputListener inputListener;
        
        public override void InstallBindings()
        {
            Bind();
        }

        private void Bind()
        {
            Container.Bind<InputListener>().FromInstance(inputListener).AsSingle();
        }
    }
}
