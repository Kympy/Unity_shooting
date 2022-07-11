using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private TextMeshProUGUI attackMode;
    private TextMeshProUGUI speed;
    private TextMeshProUGUI height;
    private Color basicColor;
    private Slider HPbar;

    private GameObject GunCrossHair;
    private GameObject MissileCrossHair;
    private Vector3 originPos;
    private Color basicColor_Aim;
    private float currentHP;

    private void Awake()
    {
        attackMode = GameObject.Find("Mode").GetComponent<TextMeshProUGUI>();
        speed = GameObject.Find("Speed").GetComponent<TextMeshProUGUI>();
        height = GameObject.Find("Height").GetComponent<TextMeshProUGUI>();
        basicColor = height.color;
    }
    private void Start()
    {
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
    public void FocusTarget(int index) // 에임을 타겟에게 자동조준
    {
        if (GameManager.Instance.Target.Count > 0) // 조준할 적이 하나라도 있으면
        {
            MissileCrossHair.transform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.Target[index].transform.position); // 타겟의 좌표를 스크린좌표로 변환
            MissileCrossHair.GetComponent<Image>().color = Color.red; // 빨간색으로 UI 변경
        }
        else
        {
            FocusOut(); // 조준할 게 없으면 조준 해제
        }
    }
    public void FocusOut() // 조준 해제
    {
        MissileCrossHair.transform.position = originPos;
        MissileCrossHair.GetComponent<Image>().color = basicColor_Aim;
    }
    public void UpdateHPbar() // 체력바 업데이트
    {
        currentHP = GameManager.Instance._Player.GetHP();
        HPbar.value = Mathf.Lerp(HPbar.value, currentHP, Time.deltaTime * 10f);
    }
}
