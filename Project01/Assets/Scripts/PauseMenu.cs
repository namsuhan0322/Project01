using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject _pauseMenu;

    public GameObject inventory;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            } 
            else 
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
        inventory.SetActive(true);
    }

    void Pause()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true; 
        inventory.SetActive(false);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        GameIsPaused = false;
        SceneManager.LoadScene("MainScene");
    }
    
    public void QuitGame(){
        Application.Quit();
    }
}

