using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Sprite[] livesSprites;
    [SerializeField]
    private Image livesImage;
    [SerializeField]
    private Text gameOverText;
    [SerializeField]
    private Text restartText;
    private int score;

    private void Start()
    {
        livesImage.sprite = livesSprites[livesSprites.Length - 1];
        Player.PlayerDeath += displayGameOver;
        gameOverText.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);
    }

    public void UpdateScore(int playerScore)
    {
        score = playerScore;
    }

    public void updateLives(int lives)
    {
        if (lives < 0)
            lives = 0;
        livesImage.sprite = livesSprites[lives];
    }

    private void displayGameOver()
    {
        if (restartText != null)
        {
            restartText.gameObject.SetActive(true);
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            StartCoroutine(flashGameOver());
        }
    }

    IEnumerator flashGameOver()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
            gameOverText.text = "GAME OVER";
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
    }
}
