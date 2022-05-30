using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;
    public float upBound = 9.25f;
    public float downBound = -2f;
    public float leftBound = -10f;
    public float rightBound = 10f;
    [SerializeField]
    private GameObject shieldPrefab;
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private GameObject tripleShotPrefab;
    [SerializeField]
    private float cooldown = 0.2f;
    [SerializeField]
    private GameObject rightDamage;
    [SerializeField]
    private GameObject leftDamage;
    [SerializeField]
    private GameObject shield;
    private float canShoot = -1;
    [SerializeField]
    private int lives = 3;

    [SerializeField]
    private Slider tripleShotSlider;
    [SerializeField]
    private Image tripleShotSliderFill;
    [SerializeField]
    private Slider shieldSlider;
    [SerializeField]
    private Image shieldSliderFill;
    [SerializeField]
    private Slider speedSlider;
    [SerializeField]
    private Image speedSliderFill;

    [SerializeField]
    private AudioClip laserShotSFX;
    [SerializeField]
    private AudioClip explosionSFX;
    [SerializeField]
    private AudioClip powerdownSFX;
    [SerializeField]
    private AudioClip powerupSFX;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource dmg1source;
    [SerializeField]
    private AudioSource dmg2source;
    private float speedMultiplier = 1f;
    private int score = 0;

    private bool speedActive = false;
    private float speedTime = 70; // in 1/10 of seconds
    private float speedTimeRemaining = 0;
    private float shieldTime = 200; // in 1/10 of seconds
    private float shieldTimeRemaining = 0;
    private int tripleShotMax = 12;
    private int tripleShotsRemaining = 0;

    [SerializeField]
    private UIManager uiManager;
    private int chooseWing = 0;

    public delegate void PlayerDies();
    public static event PlayerDies PlayerDeath;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        rightDamage.SetActive(false);
        leftDamage.SetActive(false);
        shield.SetActive(false);

        StartCoroutine(CountSeconds());
    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * speed * speedMultiplier * Time.deltaTime);
    }

    void OutOfBounds()
    {
        if (transform.position.x > leftBound && transform.position.x < rightBound && transform.position.y > downBound && transform.position.y < upBound)
        {
            return;
        }
        else
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftBound, rightBound), transform.position.y, 0);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, downBound, upBound), 0);
        }
    }

    void FireLaser()
    {
        canShoot = Time.time + cooldown;

        if (tripleShotsRemaining > 0)
        {
            Instantiate(tripleShotPrefab, new Vector3(transform.position.x, transform.position.y + 1.05f, 0), Quaternion.identity);
            uiManager.shotsFired+=3;
            updateTripleShotUI();
            tripleShotsRemaining--;
        }
        else
        {
            Instantiate(laserPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            uiManager.shotsFired++;
        }

        audioSource.PlayOneShot(laserShotSFX, 0.6f);

    }

    public void Damage()
    {
        if (shield.activeSelf)
        {
            shield.SetActive(false);
        }
        else
        {
            if (chooseWing != 0)
            {
                leftDamage.SetActive(true);
                rightDamage.SetActive(true);
            }
            else
            {
                chooseWing = Random.Range(1, 3);
                if (chooseWing == 1)
                    leftDamage.SetActive(true);
                else rightDamage.SetActive(true);
            }

            audioSource.PlayOneShot(explosionSFX, 1f);
            lives--;
            if (lives == 2 && !dmg1source.isPlaying)
                dmg1source.Play();
            else if (lives == 1 && !dmg2source.isPlaying)
                dmg2source.Play();

            uiManager.updateLives(lives);
        }
        audioSource.PlayOneShot(powerdownSFX, 0.6f);

        if (lives < 1)
        {
            PlayerDeath?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void TripleShotActivate()
    {
        tripleShotsRemaining = tripleShotMax;
        updateTripleShotUI();
        audioSource.PlayOneShot(powerupSFX, 0.8f);
    }

    public void SpeedActivate()
    {
        speedMultiplier = 2f;
        cooldown = 0f;

        audioSource.PlayOneShot(powerupSFX, 0.8f);

        if (!speedActive)
        {
            speedActive = true;
            StartCoroutine(PowerdownSpeed(false));
        }
        else
        {
            StartCoroutine(PowerdownSpeed(true));
        }
    }

    IEnumerator PowerdownSpeed(bool isStillActive)
    {
        // Wait until it turns false, then turn it back to true
        if (isStillActive)
        {
            while (speedActive == true)
            {
                yield return null;
            }
            speedActive = true;
        }

        for (speedTimeRemaining = speedTime; speedTimeRemaining > 0; speedTimeRemaining--)
        {
            updateSpeedUI();
            yield return new WaitForSeconds(0.1f);
        }
        speedMultiplier = 1f;
        cooldown = 0.2f;
        speedActive = false;
    }

    public void ShieldActivate()
    {
        audioSource.PlayOneShot(powerupSFX, 0.8f);

        if (!shield.activeSelf)
        {
            shield.SetActive(true);
            StartCoroutine(PowerdownShield(false));
        }
        else
        {
            StartCoroutine(PowerdownShield(true));
        }
    }

    IEnumerator PowerdownShield(bool resetShield)
    {
        for (shieldTimeRemaining = shieldTime; shieldTimeRemaining > 0; shieldTimeRemaining--)
        {
            if (resetShield)
            {
                shieldTimeRemaining = shieldTime;
                resetShield = false;
            }
            if (!shield.activeSelf)
                shieldTimeRemaining = 0;
            
            updateShieldUI();
            yield return new WaitForSeconds(0.1f);
        }
        shield.SetActive(false);
    }

    public void addScore(int addTo)
    {
        score += addTo;
        uiManager.UpdateScore(score);
    }

    private void updateTripleShotUI()
    {
        tripleShotSlider.value = tripleShotsRemaining - 1;
        tripleShotSlider.maxValue = tripleShotMax;

        if (tripleShotsRemaining > 1)
        {
            tripleShotSliderFill.color = Color.green;
        }
        else
        {
            tripleShotSliderFill.color = new Color(0, 0, 0, 0);
        }

    }

    private void updateShieldUI()
    {
        shieldSlider.value = shieldTimeRemaining;
        shieldSlider.maxValue = shieldTime;

        if (shieldTimeRemaining > 1)
            shieldSliderFill.color = Color.blue;
        else
            shieldSliderFill.color = new Color(0, 0, 0, 0);
    }

    private void updateSpeedUI()
    {
        speedSlider.value = speedTimeRemaining;
        speedSlider.maxValue = speedTime;

        if (speedTimeRemaining > 1)
            speedSliderFill.color = Color.red;
        else
            speedSliderFill.color = new Color(0, 0, 0, 0);
    }

    IEnumerator CountSeconds()
    {
        while (lives > 0)
        {
            yield return new WaitForSeconds(1);
            uiManager.gameTime++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        OutOfBounds();

        if (shield.activeSelf)
            updateShieldUI();
        if (speedActive)
            updateSpeedUI();

        if (Input.GetKeyDown(KeyCode.Space) && canShoot < Time.time)
            FireLaser();
    }
}
