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

    public List<GameObject> Target; // 유도가 가능한 타겟 목록
    public Camera mainCamera; // 메인 카메라
    private int score = 0; // 게임 점수
    private bool GameOver = false;

    private void Awake()
    {
        _Player = GameObject.FindObjectOfType<Player>();
        _Input = GameObject.FindObjectOfType<InputManager>();
        _UI = GameObject.FindObjectOfType<UIManager>();
        _Effect = GameObject.FindObjectOfType<EffectManager>();
        _Loading = GameObject.FindObjectOfType<LoadingSceneManager>();
        _BulletPool = GameObject.FindObjectOfType<BulletPool>();
    }
    private void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _BulletPool.Init();
        _Effect.Init();
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
}
