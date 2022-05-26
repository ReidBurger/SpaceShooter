using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.5f;
    public float upBound = 9.25f;
    public float downBound = -2f;
    public float leftBound = -10f;
    public float rightBound = 10f;

    private void Start()
    {
        Player.playerDeath += stopMoving;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Shield"))
        {
            Player player = other.transform.parent.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player"))
        { 
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Player.playerDeath -= stopMoving;
            Destroy(gameObject);
        }
    }

    private void stopMoving()
    {
        speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < downBound)
        {
            float xPos = Random.Range(leftBound + 1f, rightBound - 1f);
            transform.position = new Vector3(xPos, upBound, 0);
        }
    }
}
