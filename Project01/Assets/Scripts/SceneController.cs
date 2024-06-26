using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject settingCanvas;
    
    public void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SettingScene()
    {
        mainCanvas.SetActive(false);
        settingCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("MainScene");
        mainCanvas.SetActive(true);
        settingCanvas.SetActive(false);
    }
}
