using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Meta.UI
{
    [RequireComponent(typeof(Button))]
    public class SceneLoadingButton : MonoBehaviour
    {
        [SerializeField, Scene] private int scene;
    
        private Button _button;

    
        private void Awake()
        {
            _button = GetComponent<Button>();
            
            _button.onClick.AddListener(LoadScene);
        }

    
        private void LoadScene()
        {
            SceneManager.LoadScene(scene);
        }
    }
}
