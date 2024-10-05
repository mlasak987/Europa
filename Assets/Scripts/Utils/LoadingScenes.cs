using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Europa.Utils
{
    public class LoadingScenes : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private Image loadingBar;

        public void LoadScene(int sceneId)
        {
            StartCoroutine(LoadSceneAsync(sceneId));
        }

        IEnumerator LoadSceneAsync(int sceneId)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
            loadingScreen.SetActive(true);

            while (!operation.isDone)
            {
                float progressValue = Mathf.Clamp01((float)operation.progress / 0.9f);
                loadingBar.fillAmount = progressValue;

                yield return null;
            }
        }
    }
}
