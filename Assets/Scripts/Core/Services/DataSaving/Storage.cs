using System;
using Core.Services.DataSaving.Behaviours;
using UnityEngine;
using Zenject;

namespace Core.Services.DataSaving
{
    public class Storage : MonoBehaviour
    {
        [SerializeField] private SavingType savingType;

        private Authentication _authentication;
        
        public SavingBehaviour SavingBehaviour { get; private set; }

        
        [Inject]
        private void Construct(Authentication authentication)
        {
            _authentication = authentication;
        }

        private void OnEnable()
        {
            _authentication.SignedIn += Init;
        }

        private void OnDisable()
        {
            _authentication.SignedIn -= Init;
        }


        private void Init()
        {
            SavingBehaviour = savingType switch
            {
                SavingType.Cloud => new CloudSavingBehaviour(),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            SavingBehaviour.Init();
        }
    }
}
