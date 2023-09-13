using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private GameObject playerCharacter;

    private void Awake()
    {
        playerCharacter = Resources.Load("Player/PlayerCharacter") as GameObject;
        Instantiate(playerCharacter, new Vector3(0, 600, 0), transform.rotation);
        GameManager.Instance.currentPlayer = playerCharacter.GetComponent<Player>();
    }
}
