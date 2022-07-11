using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    private int sceneIndex;
    private float timer = 0f;
    private Slider progressBar;

    private void Awake()
    {
        progressBar = GameObject.Find("ProgressBar").GetComponent<Slider>();
    }
    private void Start()
    {
        StartCoroutine(Loading());
    }
    public void LoadSceneNumber(int index)
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
                progressBar.value = Mathf.Lerp(progressBar.value, loadingOp.progress, Time.deltaTime * 8f);
                if(progressBar.value >= loadingOp.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.value = Mathf.Lerp(progressBar.value, 1f, Time.deltaTime * 8f);
                if(progressBar.value >= 1.0f)
                {
                    loadingOp.allowSceneActivation = true;
                }
            }
        }
    }
}
