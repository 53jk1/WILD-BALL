using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +2);
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void QuitGame ()
    {
        Debug.Log("QUIT!");
        Application.Quit();

    }

    public void GoBackToMenu()
    {
        Debug.Log("BACK");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -3);
    }

    public void GoBackToMenuFromRetry()
    {
        Debug.Log("Go back to Menu from Retry");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }
}
