using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Action

public class InputManager : MonoBehaviour
{
    public Action KeyAction = null;

    private Dictionary<KeyCode, Action> ingameKeyDictionary;

    private void Start()
    {
        ingameKeyDictionary = new Dictionary<KeyCode, Action>
        {
            { KeyCode.W, GameManager.Instance._Player.MoveFoward }, // 전진
            { KeyCode.D, GameManager.Instance._Player.MoveRight }, // 우회전
            { KeyCode.A, GameManager.Instance._Player.MoveLeft }, // 좌회전
            { KeyCode.Keypad8, GameManager.Instance._Player.TiltForward }, // 전방 기울기
            { KeyCode.Keypad5, GameManager.Instance._Player.TiltBackward }, // 후방 기울기
            { KeyCode.Keypad4, GameManager.Instance._Player.TiltLeft }, // 좌측 기울기
            { KeyCode.Keypad6, GameManager.Instance._Player.TiltRight }, // 우측 기울기
            { KeyCode.Space, GameManager.Instance._Player.Shoot } // 사격
        };
        
    }
    public void UpdateIngameKey()
    {
        if (Input.anyKey)
        {
            foreach (var dic in ingameKeyDictionary)
            {
                if (Input.GetKey(dic.Key))
                {
                    dic.Value();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.R)) // 모드 변경
        {
            GameManager.Instance._Player.ChangeAttackMode();
        }
        if (Input.GetKeyUp(KeyCode.Space)) // 사격
        {
            GameManager.Instance._Player.ShootKeyUp();
        }
        if(Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.GetGameOver()) // 게임 오버 시 재시작
        {
            GameManager.Instance._Player.ResetPlayer(); // 플레이어 초기화
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.ExitGame(); // 게임 종료
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            GameManager.Instance._Player.WKeyUp(); // W 릴리즈 시에 추진기 비활성화
        }
        if(Input.GetKeyDown(KeyCode.E)) // 다음 타겟 변경
        {
            GameManager.Instance._Player.NextTargetIndex();
        }
        if (Input.GetKeyDown(KeyCode.Q)) // 이전 타겟 변경
        {
            GameManager.Instance._Player.BeforeTargetIndex();
        }
        if (Input.GetKeyDown(KeyCode.F)) // 자살
        {
            GameManager.Instance._Player.DecreaseHP(10f);
        }
        else
        {
            return;
        }
    }
}
