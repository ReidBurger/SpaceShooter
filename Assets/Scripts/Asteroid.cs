using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    // 0 is small, 1 is large
    private int asteroidType;
    [SerializeField]
    private GameObject[] powerups;
    private int asteroidDurability = 5;
    private float downBound = -2f;
    private int direction;
    private int[] directionList = new int[] { -1, 1 };
    
    Player player;
    Animator animator;
    new CircleCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        // Pick either left (-1) or right (1) for a rotation direction
        direction = directionList[Random.Range(0, 1)];
        rotateSpeed = Random.Range(15, 25);

        if (asteroidType == 1)
        {
            moveSpeed *= 0.5f;
        }

        player = GameObject.Find("Player").GetComponent<Player>();
        animator = gameObject.GetComponent<Animator>();
        collider = gameObject.GetComponent<CircleCollider2D>();

        if (player == null)
            Debug.Log("Player is null");

        if (animator == null)
            Debug.Log("Animator is null");

        if (collider == null)
            Debug.Log("Animator is null");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            if (asteroidType == 1)
            {
                if (asteroidDurability < 1)
                {
                    Destroy(collision.gameObject);
                    Explode();
                }
                else
                {
                    Destroy(collision.gameObject);
                    asteroidDurability--;
                }
            }
            else
            {
                Destroy(collision.gameObject);
                Explode();
                SpawnRandomPowerup();
            }
        }
        else if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.Damage();
                Explode();
            }
        }
    }

    void Explode()
    {
        animator.SetTrigger("OnAsteroidDestroy");
        collider.enabled = false;
        Destroy(gameObject, 2.5f);
    }

    private void SpawnRandomPowerup()
    {
        if (powerups.Length > 0)
        {
            Instantiate(powerups[Random.Range(0, powerups.Length)], new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(0, -1) * moveSpeed * Time.deltaTime, Space.World);
        transform.Rotate(new Vector3(0, 0, direction) * rotateSpeed * Time.deltaTime);

        if (transform.position.y < downBound - 2)
        {
            Destroy(gameObject);
        }
    }
}
