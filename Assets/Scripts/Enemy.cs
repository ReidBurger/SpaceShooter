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
    private Player player;
    Animator animator;
    new BoxCollider2D collider;

    private void Start()
    {
        Player.PlayerDeath += stopMoving;
        player = GameObject.Find("Player").GetComponent<Player>();

        if (player == null)
            Debug.Log("Player is null");

        animator = gameObject.GetComponent<Animator>();
        collider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            player.addScore(10);
            Destroy(other.gameObject);
            Die();
        }
        else if (other.CompareTag("Shield"))
        {
            player.Damage();
            Destroy(other.gameObject);
            Die();
        }
        else if (other.CompareTag("Player"))
        {
            player.Damage();
            Player.PlayerDeath -= stopMoving;
            Die();
        }
    }

    private void stopMoving()
    {
        speed = 1f;
    }

    private void Die()
    {
        speed = 1f;
        animator.SetTrigger("OnEnemyDeath");
        collider.enabled = false;
        Destroy(gameObject, 2.5f);
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
