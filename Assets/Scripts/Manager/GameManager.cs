using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Player _Player;      // ======= 플레이어
    public InputManager _Input;
    public UIManager _UI;
    public EffectManager _Effect;
    public BulletPool _BulletPool;

    public List<GameObject> Target = new List<GameObject>(); // 유도가 가능한 타겟 목록
    private int score = 0; // 게임 점수
    private bool GameOver = false; // 게임 오버

    private void Awake()
    {
        gameObject.AddComponent<InputManager>(); // 생성 시 컴포넌트 부착
        gameObject.AddComponent<UIManager>();
        gameObject.AddComponent<EffectManager>();
        gameObject.AddComponent<BulletPool>();

        _Input = GetComponent<InputManager>(); // 정보가져옴
        _UI = GetComponent<UIManager>();
        _Effect = GetComponent<EffectManager>();
        _BulletPool = GetComponent<BulletPool>();

        Time.timeScale = 1f;
        GameOver = false;
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) Cursor.visible = false; // 게임 씬이라면 커서 비활성화
        _Player = FindObjectOfType<Player>();
        _Player.ResetHP();
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1) // 게임 씬이라면 업데이트
        {
            _Input.OnUpdate();
            _UI.TextSpeed();
            _UI.TextHeight();
            _UI.TextScore();
            _UI.UpdateHPbar();
            CheckScore();
        }
        else Cursor.visible = true;
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
    public void InitGameManager() // 게임매니저 초기화
    {
        _Input = GetComponent<InputManager>();
        _UI = GetComponent<UIManager>();
        _Effect = GetComponent<EffectManager>();
        _BulletPool = GetComponent<BulletPool>();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void CheckScore()
    {
        if(score >= 2000)
        {
            _UI.GameWin();
        }
    }
}
