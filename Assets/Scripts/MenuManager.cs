using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static bool gameIsPaused;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
        if (Input.GetKeyDown("r"))
        {
            RestartGame();
        }
        if (Input.GetKeyDown("q"))
        {
            ExitGame();
        }
    }

    public void PauseGame()
    {
        if (gameIsPaused)
        {

        }
        else
        {

        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
