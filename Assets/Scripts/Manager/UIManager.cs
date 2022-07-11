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

    private GameObject GunCrossHair;
    private GameObject MissileCrossHair;
    private Vector3 originPos;
    private Color basicColor_Aim;

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
        Debug.Log(GameManager.Instance._Player.GetVelocity() * 50);
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
        if (GameManager.Instance.Target.Count > 0)
        {
            MissileCrossHair.transform.position = Camera.main.WorldToScreenPoint(GameManager.Instance.Target[index].transform.position);
            MissileCrossHair.GetComponent<Image>().color = Color.red;
        }
        else
        {
            FocusOut();
        }
    }
    public void FocusOut()
    {
        MissileCrossHair.transform.position = originPos;
        MissileCrossHair.GetComponent<Image>().color = basicColor_Aim;
    }
}
