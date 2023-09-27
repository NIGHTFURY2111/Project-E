using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void GameStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void GameQuit()
    {
        Application.Quit();
        Debug.Log("Game Exited");
    }
}
