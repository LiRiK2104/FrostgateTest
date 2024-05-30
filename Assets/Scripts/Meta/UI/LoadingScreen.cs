using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Meta.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField, Min(0)] private float loadingDelay = 2f;
        [SerializeField, Scene] private int nextScene;

        private void Start()
        {
            Invoke(nameof(LoadScene), loadingDelay);
        }
    
        private void LoadScene()
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
