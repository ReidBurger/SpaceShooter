using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool gameover = false;

    // Start is called before the first frame update
    void Start()
    {
        Player.PlayerDeath += isGameOver;
    }

    void isGameOver()
    {
        gameover = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && gameover)
        {
            // scene 1 is "Game" - see in File -> Build Settings
            SceneManager.LoadScene(1);
        }
    }
}
