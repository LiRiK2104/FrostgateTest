using Core.Services;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class AuthenticationInstaller : MonoInstaller
    {
        [SerializeField] private Authentication authentication;
        
        public override void InstallBindings()
        {
            Bind();
        }

        private void Bind()
        {
            Container.Bind<Authentication>().FromInstance(authentication).AsSingle();
        }
    }
}
