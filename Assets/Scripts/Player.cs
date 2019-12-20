using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int level;
    public GameObject quitPanel;
    private int gamePlayLevel;
    private void Start()
    {
        if (GameManager.self != null)
        {
            //level = 0;
            //gamePlayLevel = 0;
            //PlayerPrefs.SetInt("level", level);
            //PlayerPrefs.SetInt("gamePlayLevel", gamePlayLevel);
            level = PlayerPrefs.GetInt("level");
            gamePlayLevel = PlayerPrefs.GetInt("gamePlayLevel");
            if (level == 0)
            {
                level = 1;
            }
            if (gamePlayLevel == 0)
            {
                gamePlayLevel = 1;
            }
            GameManager.self.Restart(level, gamePlayLevel);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            quitPanel.gameObject.SetActive(true);
        }
    }

    public void quit()
    {
        if (!quitPanel)
        {
            level = GameManager.self.Level;
            PlayerPrefs.SetInt("level", level);
            quitPanel.gameObject.SetActive(false);
            SceneManager.LoadScene("Menu");
        }
    }

    public void SetPlayLevel(int levelPlay)
    {
        gamePlayLevel = levelPlay;
        PlayerPrefs.SetInt("gamePlayLevel", gamePlayLevel);
    }

}
