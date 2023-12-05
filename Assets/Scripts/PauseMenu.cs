using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject PauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown("x"))
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (GamePaused)
        {
            if (Input.GetKeyDown("a"))//Reload"))
            {
                ReLoadScene();
            }

            if (Input.GetKeyDown("y"))//Cancel"))
            {
                QuitGame();
            }
        }

        if (!GamePaused)
        {
            PauseMenuUI.SetActive(false);
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        GamePaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        GamePaused = true;
    }

    public void ReLoadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
