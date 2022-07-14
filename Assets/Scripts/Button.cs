using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(2); // ·Îµù ¾À È£Ãâ
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
