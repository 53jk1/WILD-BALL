using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;

    void OnCollisionEnter (Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Obstacle") {

            movement.enabled = false;
            FindObjectOfType<GameManager>().EndGame();
            
            

            

        }

            if (collisionInfo.collider.tag == "Finish") {

            movement.enabled = false;
            FindObjectOfType<GameManager>().FinishGame();

            

        }
    }
}
