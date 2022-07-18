using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    private static int sceneIndex = 1; // 로딩 할 씬
    private float timer = 0f;
    public Image progressBar;

    private void Start()
    {
        StartCoroutine(Loading()); // 로딩 씬 진입 시 로딩 코루틴 시작
    }
    public static void LoadSceneNumber(int index)
    {
        sceneIndex = index;
        SceneManager.LoadScene(2); // 로딩 씬 호출
    }
    IEnumerator Loading()
    {
        yield return null;
        AsyncOperation loadingOp = SceneManager.LoadSceneAsync(sceneIndex); // 비동기 로딩
        loadingOp.allowSceneActivation = false;

        while(loadingOp.isDone == false) // 로딩이 진행 중이라면
        {
            timer += Time.deltaTime;
            if(loadingOp.progress < 0.9f) // 로딩 진행률이 90 퍼센트 미만이라면
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, loadingOp.progress, timer); // Bar 를 로딩 비율만큼 채움
                if(progressBar.fillAmount >= loadingOp.progress) // Bar 가 로딩 비율보다 크다면
                {
                    timer = 0f; // 타이머 정지
                }
            }
            else // 로딩이 90 퍼센트 이상이라면
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer); // 100퍼센트까지 부드럽게 채워줌
                if(progressBar.fillAmount >= 1.0f) // 100퍼센트 이상이라면
                {
                    loadingOp.allowSceneActivation = true; // 씬 로드 허용
                    yield break;
                }
            }
        }
    }
}
