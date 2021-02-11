using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;

    public float restartDelay = 100000f;

    public void EndGame () {

        if (gameHasEnded == false){
            gameHasEnded = true;
            Debug.Log("GAME OVER");
            Restart();
        }
    }

    void Restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }

    public void FinishGame () {

        if (gameHasEnded == false){
            gameHasEnded = true;
            Debug.Log("YOU WON");
            Victory();
        }
    }

    void Victory ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
}
