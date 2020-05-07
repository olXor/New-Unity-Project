using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace cg {
    public class SceneController : MonoBehaviour {
        public bool preloadLoaded { get; private set; } = false;

        void Awake() {
            Scene preloadScene = SceneManager.GetSceneByName("PreloadScene");
            if (!preloadScene.isLoaded)
                SceneManager.LoadScene("PreloadScene", LoadSceneMode.Additive);
            StartCoroutine(checkPreload());
        }

        private IEnumerator checkPreload() {
            Scene preloadScene = SceneManager.GetSceneByName("PreloadScene");
            yield return new WaitUntil(() => preloadScene.isLoaded);
            preloadLoaded = true;
        }

        public void swapScene(string sceneName) {
            StartCoroutine(swapSceneCoroutine(sceneName));
        }

        public IEnumerator swapSceneCoroutine(string sceneName) {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            Scene newScene = SceneManager.GetSceneByName(sceneName);
            yield return new WaitUntil(() => newScene.isLoaded);
            Scene prevScene = SceneManager.GetActiveScene();
            SceneManager.SetActiveScene(newScene);
            SceneManager.UnloadSceneAsync(prevScene);
        }
    }
}
