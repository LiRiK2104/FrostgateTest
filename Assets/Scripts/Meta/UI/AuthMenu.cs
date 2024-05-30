using System;
using Core.Services;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Meta.UI
{
    public class AuthMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private TextMeshProUGUI idLabel;
        [Space] 
        [SerializeField, Scene] private int gameScene; 
    
        private Authentication _authentication;
    
    
        [Inject]
        private void Construct(Authentication authentication)
        {
            _authentication = authentication;
        }

        private void Awake()
        {
            playButton.onClick.AddListener(() => SceneManager.LoadScene(gameScene));
        }

        private void OnEnable()
        {
            _authentication.SignedIn += UpdateView;
        }

        private void OnDisable()
        {
            _authentication.SignedIn -= UpdateView;
        }

        private void Start()
        {
            UpdateView();

            if (_authentication.IsAuthorized == false) 
                _authentication.SignInAnonymous();
        }


        private void UpdateView()
        {
            string playerId = _authentication.IsAuthorized ? _authentication.PlayerId : "---";
            idLabel.text = $"Player Id: {playerId}";
        
            playButton.gameObject.SetActive(_authentication.IsAuthorized);
        }
    }
}
