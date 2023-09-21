using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UtilFunction;
public class GameManager : Singleton<GameManager>, IInitialize
{
    public Player currentPlayer;      // ======= 플레이어
    public UIManager UI;
    public EffectManager _Effect;
    public GameObjectPool _BulletPool;

    public List<GameObject> Target = new List<GameObject>(); // 유도가 가능한 타겟 목록
    private int score = 0; // 게임 점수
    private bool GameOver = false; // 게임 오버

    public static Camera mainCamera = null;
    // 현재 씬
    private ManagedSceneIndex currentSceneIndex = ManagedSceneIndex.Title;
    public bool IsInitialized { get; private set; } = false;
    public async Task Initialize()
    {
        try
        {
		    // 기본 하위 매니저 생성
		    await InitDefaultManager();
        }
        catch (Exception ex) 
        { 
            Debug.LogError(ex.ToString());
            IsInitialized = false; 
        }
        IsInitialized = true;
	}
    protected override async void Awake()
    {
        base.Awake();
        UtilFunction.PlayTime();
        await Initialize();
        LoadScene((ManagedSceneIndex)SceneManager.GetActiveScene().buildIndex);
    }
    private async Task InitDefaultManager()
    {
        UI = AddAndGetComponent<UIManager>(dontDestroyOnLoad: true);
        await UI.Initialize();
	}
    public int GetScore()
    {
        return score;
    }
    public void SetScore(int plus)
    {
        score += plus;
    }
    public bool GetGameOver()
    {
        return GameOver;
    }
    public void SetGameOver(bool b) // 게임오버 변수
    {
        GameOver = b;
    }
    public List<GameObject> GetTargetList() // 타겟 정보 반환
    {
        return Target;
    }
    public void AddTargetList(GameObject target) // 타겟리스트에 추가
    {
        Target.Add(target);
    }
    public void RemoveTargetList(GameObject target) // 리스트에서 타겟 제거
    {
        Target.Remove(target);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void CheckScore()
    {
        if(score >= 2000)
        {
            UI.GameWin();
        }
    }
    public SceneObject CurrentSceneObject { get; private set; } = null;
    public T GetCurrentSceneObject<T>() where T : SceneObject
    {
        return CurrentSceneObject as T;
    }
    public async void LoadScene(ManagedSceneIndex targetScene, Action loadFinishAction = null)
    {
        if ((int)targetScene == SceneManager.GetActiveScene().buildIndex)
        {
            CreateSceneObject(targetScene);
            return;
        }
        UI.ToggleLoadingUI(true);

        AsyncOperation loadingHandle = SceneManager.LoadSceneAsync((int)targetScene);
        loadingHandle.allowSceneActivation = false;

        while(loadingHandle.isDone == false)
        {
            // Yield() 가 맞지만 로딩을 그럴듯하게 하기 위해 의도적으로 딜레이를 줌.
            await Task.Delay(100);
            // await Task.Yield();
            UI.NotifyLoadingProgressValue(loadingHandle.progress);
            if (loadingHandle.progress >= 0.9f)
            {
                loadingHandle.allowSceneActivation = true;
                break;
            }
        }
        while(SceneManager.GetActiveScene().buildIndex != (int)targetScene)
        {
            //Debug.Log(SceneManager.GetActiveScene().buildIndex);
            await Task.Yield();
        }
		//Debug.Log(SceneManager.GetActiveScene().buildIndex);
        CreateSceneObject(targetScene);
        currentSceneIndex = targetScene;
		loadFinishAction?.Invoke();
	}
    private void CreateSceneObject(ManagedSceneIndex targetScene)
    {
		if (CurrentSceneObject != null)
		{
			UtilFunction.DestroyIfNotNull(CurrentSceneObject.gameObject);
		}
		switch (targetScene)
		{
			default:
			case ManagedSceneIndex.Title:
				{
					CurrentSceneObject = new GameObject(typeof(TitleSceneObject).Name, typeof(TitleSceneObject)).GetComponent<TitleSceneObject>();
					break;
				}
			case ManagedSceneIndex.Ingame:
				{
					CurrentSceneObject = new GameObject(typeof(IngameSceneObject).Name, typeof(IngameSceneObject)).GetComponent<IngameSceneObject>();
					break;
				}
		}
		CurrentSceneObject.InitScene();
	}
}
