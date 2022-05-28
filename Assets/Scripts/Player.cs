using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float canShoot = -1;
    [SerializeField]
    private int lives = 3;
    private float speedMultiplier = 1f;
    private bool speedActive = false;
    private bool shieldActive = false;
    private float speedTime = 7;
    private int score = 0;
    private int tripleShotsRemaining = 0;
    private int tripleShotMax = 12;
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
            tripleShotsRemaining--;
        }
        else { Instantiate(laserPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity); }

    }

    public void Damage()
    {
        if (shieldActive)
        {
            shieldActive = false;
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
            
            lives--;
            uiManager.updateLives(lives);
        }
            
        if (lives < 1)
        {
            PlayerDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    public void TripleShotActivate()
    {
        tripleShotsRemaining = tripleShotMax;
    }

    public void SpeedActivate()
    {
        speedMultiplier = 2f;
        cooldown = 0f;

        bool isStillActive = false;
        if (speedActive == true)
        {
            isStillActive = true;
        }

        speedActive = true;
        StartCoroutine(PowerdownSpeed(isStillActive));
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

        yield return new WaitForSeconds(speedTime);
        speedMultiplier = 1f;
        cooldown = 0.2f;
        speedActive = false;
    }

    public void ShieldActivate()
    {
        if (!shieldActive)
        {
            shieldActive = true;
            GameObject shield = Instantiate(shieldPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            shield.transform.parent = transform;
        }
    }

    public void addScore(int addTo)
    {
        score += addTo;
        uiManager.UpdateScore(score);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        OutOfBounds();

        if (Input.GetKeyDown(KeyCode.Space) && canShoot < Time.time)
            FireLaser();
    }
}
