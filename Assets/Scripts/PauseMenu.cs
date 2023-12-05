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
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKey("options") || Input.GetKey("settings") || Input.GetKey("a"))
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
            if (Input.GetKeyDown("r"))//Reload"))
            {
                ReLoadScene();
            }

            if (Input.GetKeyDown("q"))//Cancel"))
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
        //Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        GamePaused = true;
    }

    public void ReLoadScene()
    {
        //Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
