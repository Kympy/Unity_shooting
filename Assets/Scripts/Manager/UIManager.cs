using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private TextMeshProUGUI attackMode; // 공격 모드
    private TextMeshProUGUI speed; // 속도
    private TextMeshProUGUI height; // 고도
    private TextMeshProUGUI score; // 점수
    private TextMeshProUGUI gameOver; // 게임 종료화면
    private TextMeshProUGUI gameWin; // 승리화면
    private Color basicColor; // 기본 테마 초록색
    private Slider HPbar; // 체력 바

    private GameObject GunCrossHair; // 에임
    private GameObject MissileCrossHair; // 미사일 에임
    private Vector3 originPos; // 미사일 UI 기본 위치
    private Color basicColor_Aim; // 기본 색
    private float currentHP; // 현재 체력

    private void Awake()
    {
        attackMode = GameObject.Find("Mode").GetComponent<TextMeshProUGUI>();
        speed = GameObject.Find("Speed").GetComponent<TextMeshProUGUI>();
        height = GameObject.Find("Height").GetComponent<TextMeshProUGUI>();
        score = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        gameOver = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        gameWin = GameObject.Find("GameWin").GetComponent<TextMeshProUGUI>();
        basicColor = height.color;
    }
    private void Start()
    {
        gameOver.gameObject.SetActive(false);
        gameWin.gameObject.SetActive(false);
        GunCrossHair = GameObject.Find("CrossHairGun").gameObject;
        MissileCrossHair = GameObject.Find("CrossHairMissile").gameObject;
        MissileCrossHair.SetActive(false);
        originPos = MissileCrossHair.transform.position;
        basicColor_Aim = MissileCrossHair.GetComponent<Image>().color;
        HPbar = FindObjectOfType<Slider>();
        HPbar.maxValue = 100f;
        currentHP = GameManager.Instance._Player.GetHP();
    }
    // ============================== 공격 모드 텍스트 UI ============================ //
    public void TextAttackMode()
    {
        if(GameManager.Instance._Player.GetAttackMode()) // 미사일 모드라면
        {
            attackMode.text = "MISSILE MODE";
            MissileCrossHair.SetActive(true);
            GunCrossHair.SetActive(false);
        }
        else
        {
            attackMode.text = "MACHINE GUN MODE";
            MissileCrossHair.SetActive(false);
            GunCrossHair.SetActive(true);
        }
    }
    public void TextSpeed() // 속도 표시
    {
        speed.text = "SPEED : " + Mathf.Round((GameManager.Instance._Player.GetVelocity() * 50) * 10 / 10) + " km/s";
        //Debug.Log(GameManager.Instance._Player.GetVelocity() * 50);
    }
    public void TextHeight() // 고도 표시
    {
        if(GameManager.Instance._Player.transform.position.y < 150.0f)
        {
            height.color = Color.red;
            height.text = "HEIGHT : " + Mathf.Round(GameManager.Instance._Player.transform.position.y * 100) / 100 + " m";
        }
        else if(GameManager.Instance._Player.transform.position.y < 230.0f)
        {
            height.color = Color.yellow;
            height.text = "HEIGHT : " + Mathf.Round(GameManager.Instance._Player.transform.position.y * 100) / 100 + " m";
        }
        else
        {
            height.color = basicColor;
            height.text = "HEIGHT : " + Mathf.Round(GameManager.Instance._Player.transform.position.y * 100) / 100 + " m";
        }
    }
    public void TextScore()
    {
        score.text = "SCORE : " + GameManager.Instance.GetScore();
    }
    public void FocusTarget(int index) // 에임을 타겟에게 자동조준
    {
        if (GameManager.Instance.GetTargetList().Count > 0) // 조준할 적이 하나라도 있으면
        {
            if(GameManager.Instance.GetTargetList()[index] == null) // 타겟이 null 이면
            {
                FocusOut(); // 조준 해제
            }
            else
            {
                MissileCrossHair.transform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.GetTargetList()[index].transform.position); // 타겟의 좌표를 스크린좌표로 변환
                MissileCrossHair.GetComponent<Image>().color = Color.red; // 빨간색으로 UI 변경
            }
        }
        else
        {
            FocusOut(); // 조준할 게 없으면 조준 해제
        }
    }
    public void FocusOut() // 조준 해제
    {
        if(MissileCrossHair != null)
        {
            MissileCrossHair.GetComponent<Image>().color = basicColor_Aim;
            MissileCrossHair.transform.position = originPos;
            GameManager.Instance._Player.ResetTargetIndex();
        }
    }
    public void UpdateHPbar() // 체력바 업데이트
    {
        currentHP = GameManager.Instance._Player.GetHP();
        HPbar.value = Mathf.Lerp(HPbar.value, currentHP, Time.deltaTime * 10f);
    }
    public void GameOver() // 게임 오버 화면
    {
        GameManager.Instance.SetGameOver(true);
        gameOver.gameObject.SetActive(true);
        Time.timeScale = 0f; // 정지
    }
    public void GameWin()
    {
        GameManager.Instance.SetGameOver(true);
        gameWin.gameObject.SetActive(true);
        Time.timeScale = 0f; // 정지
    }
    public void GameRestart()
    {
        GameManager.Instance.SetGameOver(false);
        //gameOver = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        gameOver.gameObject.SetActive(false);
        gameWin.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }
}
