using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public void GotoGameScene(int mapNumber)
    {
        SceneManager.LoadScene("Game");
        PlayerPrefs.SetInt("mapNumber", mapNumber);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
