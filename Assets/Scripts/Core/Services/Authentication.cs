using System;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Core.Services
{
    public class Authentication : MonoBehaviour
    {
        internal event Action Initialized;
        internal event Action SignedIn;

        internal bool IsAuthorized => AuthenticationService.Instance.IsAuthorized;
        internal string PlayerId => AuthenticationService.Instance.PlayerId;
        

        private async void Awake()
        {
            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += InvokeOnSignedIn;
            
            InvokeOnInitialized();
        }

        private void OnDestroy()
        {
            AuthenticationService.Instance.SignedIn -= InvokeOnSignedIn;
        }


        internal async UniTask SignInAnonymous()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    
        private void InvokeOnInitialized()
        {
            Initialized?.Invoke();
        }
        
        private void InvokeOnSignedIn()
        {
            SignedIn?.Invoke();
        }
    }
}
