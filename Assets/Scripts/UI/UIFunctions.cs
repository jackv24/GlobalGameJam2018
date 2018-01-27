using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFunctions : MonoBehaviour
{
    public CanvasGroup group;

    public float fadeTime = 0.25f;

    public void LoadScene(int index)
    {
        group.interactable = false;

        StartCoroutine(LoadSceneAsync(index));
    }

    IEnumerator LoadSceneAsync(int index)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(index);
        async.allowSceneActivation = false;

        float startAlpha = group.alpha;
        float elapsed = 0;

        while(elapsed <= fadeTime)
        {
            group.alpha = Mathf.Lerp(startAlpha, 0, elapsed / fadeTime);

            yield return null;
            elapsed += Time.deltaTime;
        }

        group.alpha = 0;

        async.allowSceneActivation = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
