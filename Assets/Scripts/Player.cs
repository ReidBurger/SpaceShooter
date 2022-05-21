using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;
    public float leftBound = -10;
    public float rightBound = 10;
    public float upBound = 10;
    public float downBound = -10;
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private int cooldown = 5;
    private int cooldownTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement();
        outOfBounds();

        bool canShoot = false;
        if (cooldownTimer == 0)
        {
            canShoot = true;
        }
        else
        {
            cooldownTimer--;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            Instantiate(laserPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            cooldownTimer = cooldown;
        }
    }

    void playerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * speed * Time.deltaTime);
    }

    void outOfBounds()
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
}
