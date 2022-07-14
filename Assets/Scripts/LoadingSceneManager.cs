using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    private static int sceneIndex = 1;
    private float timer = 0f;
    public Image progressBar;

    private void Start()
    {
        StartCoroutine(Loading());
    }
    public static void LoadSceneNumber(int index)
    {
        sceneIndex = index;
        SceneManager.LoadScene(2); // ·Îµù ¾À È£Ãâ
    }
    IEnumerator Loading()
    {
        yield return null;
        AsyncOperation loadingOp = SceneManager.LoadSceneAsync(sceneIndex);
        loadingOp.allowSceneActivation = false;

        while(loadingOp.isDone == false)
        {
            timer += Time.deltaTime;
            if(loadingOp.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, loadingOp.progress, timer);
                if(progressBar.fillAmount >= loadingOp.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if(progressBar.fillAmount >= 1.0f)
                {
                    loadingOp.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
