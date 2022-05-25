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
    private GameObject laserPrefab;
    [SerializeField]
    private GameObject tripleShotPrefab;
    [SerializeField]
    private float cooldown = 0.2f;
    private float canShoot = -1;
    [SerializeField]
    private int lives = 3;
    private bool tripleShotActive = false;
    private float powerupTime = 5;

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

        transform.Translate(direction * speed * Time.deltaTime);
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
        lives--;
        if (lives < 1)
        {
            if (playerDeath != null)
                playerDeath();
            Destroy(this.gameObject);

        }

        Debug.Log("Lives Remaining: " + lives);
    }

    public void TripleShotActivate()
    {
        tripleShotActive = true;
        StartCoroutine(PowerdownTripleShot());
    }

    IEnumerator PowerdownTripleShot()
    {
        yield return new WaitForSeconds(powerupTime);
        tripleShotActive = false;
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
