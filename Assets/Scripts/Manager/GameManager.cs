using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player _Player;      // ======= 플레이어
    public InputManager _Input;
    public UIManager _UI;
    public EffectManager _Effect;
    public LoadingSceneManager _Loading;
    public BulletPool _BulletPool;

    private List<GameObject> Target = new List<GameObject>(); // 유도가 가능한 타겟 목록
    //public Camera mainCamera; // 메인 카메라
    private int score = 0; // 게임 점수
    private bool GameOver = false;

    private void Awake()
    {
        gameObject.AddComponent<InputManager>();
        gameObject.AddComponent<UIManager>();
        gameObject.AddComponent<EffectManager>();
        //gameObject.AddComponent<LoadingSceneManager>();
        gameObject.AddComponent<BulletPool>();

        _Player = FindObjectOfType<Player>();
        _Input = GetComponent<InputManager>();
        _UI = GetComponent<UIManager>();
        _Effect = GetComponent<EffectManager>();
        //_Loading = GetComponent<LoadingSceneManager>();
        _BulletPool = GetComponent<BulletPool>();
    }
    private void Start()
    {
        //mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //_BulletPool.Init();
        //_Effect.Init();
    }
    private void Update()
    {
        _Input.OnUpdate();
        _UI.TextSpeed();
        _UI.TextHeight();
        _UI.TextScore();
        _UI.UpdateHPbar();
        //Debug.Log("Target List Count : " + Target.Count);
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
    public void SetGameOver()
    {
        GameOver = true;
    }
    public List<GameObject> GetTargetList() // 타겟 정보 반환
    {
        return Target;
    }
    public void AddTargetList(GameObject target)
    {
        Target.Add(target);
    }
    public void RemoveTargetList(GameObject target)
    {
        Target.Remove(target);
    }
}
