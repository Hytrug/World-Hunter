using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Slider loadingBarSlider;

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while(!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress/0.9f);

            loadingBarSlider.value = progressValue;

            yield return null;
        }
    }
}
