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
        //ingameKeyDictionary = new Dictionary<KeyCode, Action>
        //{
        //    { KeyCode.W, GameManager.Instance.currentPlayer.MoveFoward }, // 전진
        //    { KeyCode.D, GameManager.Instance.currentPlayer.MoveRight }, // 우회전
        //    { KeyCode.A, GameManager.Instance.currentPlayer.MoveLeft }, // 좌회전
        //    { KeyCode.Keypad8, GameManager.Instance.currentPlayer.TiltForward }, // 전방 기울기
        //    { KeyCode.Keypad5, GameManager.Instance.currentPlayer.TiltBackward }, // 후방 기울기
        //    { KeyCode.Keypad4, GameManager.Instance.currentPlayer.TiltLeft }, // 좌측 기울기
        //    { KeyCode.Keypad6, GameManager.Instance.currentPlayer.TiltRight }, // 우측 기울기
        //    { KeyCode.Space, GameManager.Instance.currentPlayer.Shoot } // 사격
        //};
        
    }
    public void UpdateIngameKey()
    {
        //if (Input.anyKey)
        //{
        //    foreach (var dic in ingameKeyDictionary)
        //    {
        //        if (Input.GetKey(dic.Key))
        //        {
        //            dic.Value();
        //        }
        //    }
        //}
        //if(Input.GetKeyDown(KeyCode.R)) // 모드 변경
        //{
        //    GameManager.Instance.currentPlayer.ChangeAttackMode();
        //}
        //if (Input.GetKeyUp(KeyCode.Space)) // 사격
        //{
        //    GameManager.Instance.currentPlayer.ReleaseGunShoot();
        //}
        //if(Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.GetGameOver()) // 게임 오버 시 재시작
        //{
        //    GameManager.Instance.currentPlayer.ResetPlayer(); // 플레이어 초기화
        //}
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    GameManager.Instance.ExitGame(); // 게임 종료
        //}
        //if(Input.GetKeyUp(KeyCode.W))
        //{
        //    GameManager.Instance.currentPlayer.DisableExhaustOutlet(); // W 릴리즈 시에 추진기 비활성화
        //}
        //if(Input.GetKeyDown(KeyCode.E)) // 다음 타겟 변경
        //{
        //    GameManager.Instance.currentPlayer.NextTargetIndex();
        //}
        //if (Input.GetKeyDown(KeyCode.Q)) // 이전 타겟 변경
        //{
        //    GameManager.Instance.currentPlayer.BeforeTargetIndex();
        //}
        //if (Input.GetKeyDown(KeyCode.F)) // 자살
        //{
        //    GameManager.Instance.currentPlayer.DecreaseHP(10f);
        //}
        //else
        //{
        //    return;
        //}
    }
}
