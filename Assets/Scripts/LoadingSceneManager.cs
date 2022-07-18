using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    private static int sceneIndex = 1; // �ε� �� ��
    private float timer = 0f;
    public Image progressBar;

    private void Start()
    {
        StartCoroutine(Loading()); // �ε� �� ���� �� �ε� �ڷ�ƾ ����
    }
    public static void LoadSceneNumber(int index)
    {
        sceneIndex = index;
        SceneManager.LoadScene(2); // �ε� �� ȣ��
    }
    IEnumerator Loading()
    {
        yield return null;
        AsyncOperation loadingOp = SceneManager.LoadSceneAsync(sceneIndex); // �񵿱� �ε�
        loadingOp.allowSceneActivation = false;

        while(loadingOp.isDone == false) // �ε��� ���� ���̶��
        {
            timer += Time.deltaTime;
            if(loadingOp.progress < 0.9f) // �ε� ������� 90 �ۼ�Ʈ �̸��̶��
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, loadingOp.progress, timer); // Bar �� �ε� ������ŭ ä��
                if(progressBar.fillAmount >= loadingOp.progress) // Bar �� �ε� �������� ũ�ٸ�
                {
                    timer = 0f; // Ÿ�̸� ����
                }
            }
            else // �ε��� 90 �ۼ�Ʈ �̻��̶��
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer); // 100�ۼ�Ʈ���� �ε巴�� ä����
                if(progressBar.fillAmount >= 1.0f) // 100�ۼ�Ʈ �̻��̶��
                {
                    loadingOp.allowSceneActivation = true; // �� �ε� ���
                    yield break;
                }
            }
        }
    }
}
