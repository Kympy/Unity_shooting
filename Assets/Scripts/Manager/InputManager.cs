using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Action

public class InputManager : MonoBehaviour
{
    public Action KeyAction = null;

    Dictionary<KeyCode, Action> keyDictionary;

    private void Start()
    {
        keyDictionary = new Dictionary<KeyCode, Action>
        {
            { KeyCode.W, GameManager.Instance._Player.MoveFoward },
            { KeyCode.D, GameManager.Instance._Player.MoveRight },
            { KeyCode.A, GameManager.Instance._Player.MoveLeft },
            { KeyCode.Keypad8, GameManager.Instance._Player.TiltForward },
            { KeyCode.Keypad5, GameManager.Instance._Player.TiltBackward },
            { KeyCode.Keypad4, GameManager.Instance._Player.TiltLeft },
            { KeyCode.Keypad6, GameManager.Instance._Player.TiltRight },
            { KeyCode.Space, GameManager.Instance._Player.Shoot }
        };
        
    }
    public void OnUpdate()
    {
        if (Input.anyKey)
        {
            foreach (var dic in keyDictionary)
            {
                if (Input.GetKey(dic.Key))
                {
                    dic.Value();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance._Player.ChangeAttackMode();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GameManager.Instance._Player.ShootKeyUp();
        }
        if(Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.GetGameOver())
        {
            //GameManager.Instance._Loading.LoadSceneNumber(1);
            LoadingSceneManager.LoadSceneNumber(1);
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            GameManager.Instance._Player.WKeyUp();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance._Player.NextTargetIndex();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance._Player.BeforeTargetIndex();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.Instance._Player.DecreaseHP(10f);
        }
        else
        {
            return;
        }
        /*
        // ======= 키 입력이 없을 때 ======= //
        if (Input.anyKey == false)
        {
            return;
        }
        // ======= 키 입력이 있을 때 ======= //
        if (KeyAction != null)
        {
            KeyAction.Invoke();
        }*/

    }
}
