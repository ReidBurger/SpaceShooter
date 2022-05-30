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
    private UIManager uiManager;
    private AudioSource audioSource;
    Animator animator;
    new BoxCollider2D collider;

    private void Start()
    {
        Player.PlayerDeath += stopMoving;
        player = GameObject.Find("Player").GetComponent<Player>();
        audioSource = gameObject.GetComponent<AudioSource>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (player == null)
            Debug.Log("Player is null");
        if (audioSource == null)
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
            uiManager.kills++;
            Die();
        }
        else if (other.CompareTag("Shield"))
        {
            player.Damage();
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
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
        Destroy(gameObject, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < downBound)
        {
            uiManager.enemiesPassed++;
            float xPos = Random.Range(leftBound + 1f, rightBound - 1f);
            transform.position = new Vector3(xPos, upBound, 0);
        }
    }
}
