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
    private float canShoot = -1;
    [SerializeField]
    private int lives = 3;
    private float speedMultiplier = 1f;
    private bool tripleShotActive = false;
    private bool speedActive = false;
    private bool shieldActive = false;
    private float tripleShotTime = 5;
    private float speedTime = 7;

    public delegate void PlayerDies();
    public static event PlayerDies playerDeath;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (speedActive)
            speedMultiplier = 2f;
        else speedMultiplier = 1f;

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

        if (!tripleShotActive)
        { Instantiate(laserPrefab, new Vector3(transform.position.x, transform.position.y + 1.05f, 0), Quaternion.identity); }
        else { Instantiate(tripleShotPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity); }

    }

    public void Damage()
    {
        if (shieldActive)
        {
            shieldActive = false;
        }
        else
        {
            lives--;
        }
            
        if (lives < 1)
        {
            if (playerDeath != null)
                playerDeath();
            Destroy(gameObject);

        }

        Debug.Log("Lives Remaining: " + lives);
    }

    public void TripleShotActivate()
    {
        bool isStillActive = false;
        if (tripleShotActive == true)
        {
            isStillActive = true;
        }

        tripleShotActive = true;
        StartCoroutine(PowerdownTripleShot(isStillActive));
    }

    IEnumerator PowerdownTripleShot(bool isStillActive)
    {
        // Wait until it turns false, then turn it back to true
        if (isStillActive)
        {
            while (tripleShotActive == true)
            {
                yield return null;
            }
            tripleShotActive = true;
        }

        yield return new WaitForSeconds(tripleShotTime);
        tripleShotActive = false;
    }

    public void SpeedActivate()
    {
        bool isStillActive = false;
        if (tripleShotActive == true)
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

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        OutOfBounds();

        if (Input.GetKeyDown(KeyCode.Space) && canShoot < Time.time)
            FireLaser();
    }
}
