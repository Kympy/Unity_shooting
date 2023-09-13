using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    private static int sceneIndex = 0;
    private float timer = 0f;
    public Image progressBar;
    // 로딩 비동기 작업
    private AsyncOperation loadingOperation = null;
    public bool IsFinished
    {
        get
        {
            return loadingOperation.isDone;
        }
    }

    private void Start()
    {
        StartCoroutine(Loading());
    }
    public static void LoadSceneNumber(int targetSceneIndex)
    {
        sceneIndex = targetSceneIndex;
        SceneManager.LoadScene(targetSceneIndex); // 로딩 씬 호출
    }
    private IEnumerator Loading()
    {
        yield return null;
		loadingOperation = SceneManager.LoadSceneAsync(sceneIndex);
		loadingOperation.allowSceneActivation = false;

        while(loadingOperation.isDone == false)
        {
            timer += Time.deltaTime;
            if(loadingOperation.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, loadingOperation.progress, timer);
                if(progressBar.fillAmount >= loadingOperation.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if(progressBar.fillAmount >= 1.0f)
                {
					loadingOperation.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
