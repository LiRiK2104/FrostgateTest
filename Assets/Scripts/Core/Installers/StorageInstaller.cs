using Core.Services.DataSaving;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class StorageInstaller : MonoInstaller
    {
        [SerializeField] private Storage storage;
        
        public override void InstallBindings()
        {
            Bind();
        }

        private void Bind()
        {
            Container.Bind<Storage>().FromInstance(storage).AsSingle();
        }
    }
}