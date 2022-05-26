using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float speed = 2;
    public float downBound = -2f;
    // 0 = Triple Shot
    // 1 = Speed
    // 2 = Shield
    [SerializeField]
    private int powerupID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player  = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                if (powerupID == 0)
                    player.TripleShotActivate();
                else if (powerupID == 1)
                    player.SpeedActivate();
                else if (powerupID == 2)
                    player.ShieldActivate();
            }
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y < downBound - 1)
            Destroy(this.gameObject);
    }
}
