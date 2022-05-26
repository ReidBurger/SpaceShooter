using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject enemy_container;
    [SerializeField]
    private GameObject[] powerups;
    public float upBound = 9.25f;
    public float leftBound = -10f;
    public float rightBound = 10f;
    private bool stopSpawning = false;
   

    IEnumerator EnemySpawnRoutine()
    {
        while (!stopSpawning)
        {
            float xRange = Random.Range(leftBound + 1, rightBound - 1);
            GameObject newEnemy = Instantiate(enemy, new Vector3(xRange, upBound, 0), Quaternion.identity);
            newEnemy.transform.parent = enemy_container.transform;
            yield return new WaitForSeconds(Random.Range(0.5f, 2));
        }
    }

    IEnumerator PowerupSpawnRoutine()
    {
        while (!stopSpawning)
        {
            float xRange = Random.Range(leftBound + 1, rightBound - 1);
            Instantiate(powerups[Random.Range(0, powerups.Length)], new Vector2(xRange, upBound), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(4, 15));
        }
    }

    private void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupSpawnRoutine());
        Player.playerDeath += onPlayerDeath;
    }

    public void onPlayerDeath()
    {
        stopSpawning = true;
    }
}
