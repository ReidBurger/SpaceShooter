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
    [SerializeField]
    private Text endgameStatsTitle;
    [SerializeField]
    private Text endgameStatsNumbers;
    private int score;

    public int kills;
    public int asteroidsDestroyed;
    public int shotsFired;
    public int enemiesPassed;
    public int gameTime;

    private void Start()
    {
        livesImage.sprite = livesSprites[livesSprites.Length - 1];
        Player.PlayerDeath += displayGameOver;
        gameOverText.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);
        endgameStatsNumbers.gameObject.SetActive(false);
        endgameStatsTitle.gameObject.SetActive(false);
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

        if (endgameStatsTitle != null)
        {
            endgameStatsTitle.gameObject.SetActive(true);
        }

        if (endgameStatsNumbers != null)
        {
            float accuracy = Mathf.Round((kills + asteroidsDestroyed) * 10000f / shotsFired) / 100f;
            float mortality = Mathf.Round(kills * 10000f / (enemiesPassed + kills)) / 100f;
            float killsPerSecond = Mathf.Round(kills * 100f / gameTime) / 100f;

            Text scores = endgameStatsNumbers.GetComponent<Text>();
            scores.text = kills + "\n" + accuracy + " %\n" + mortality + " %\n" + asteroidsDestroyed + "\n" + gameTime + " s\n" + killsPerSecond;
            endgameStatsNumbers.gameObject.SetActive(true);
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
