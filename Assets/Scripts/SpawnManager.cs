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
    private GameObject SM_Asteroid;
    [SerializeField]
    private GameObject LG_Asteroid;
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
            yield return new WaitForSeconds(Random.Range(0.4f, 1.3f));
        }
    }

    IEnumerator SM_AsteroidSpawnRoutine()
    {
        while (!stopSpawning)
        {
            yield return new WaitForSeconds(Random.Range(4, 15));
            float xRange = Random.Range(leftBound + 1, rightBound - 1);
            Instantiate(SM_Asteroid, new Vector2(xRange, upBound), Quaternion.identity);
        }
    }

    IEnumerator LG_AsteroidSpawnRoutine()
    {
        while (!stopSpawning)
        {
            yield return new WaitForSeconds(Random.Range(10, 20));
            float xRange = Random.Range(leftBound + 1, rightBound - 1);
            Instantiate(LG_Asteroid, new Vector2(xRange, upBound + 3), Quaternion.identity);
        }
    }

    private void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(SM_AsteroidSpawnRoutine());
        StartCoroutine(LG_AsteroidSpawnRoutine());
        Player.PlayerDeath += onPlayerDeath;
    }

    public void onPlayerDeath()
    {
        stopSpawning = true;
    }
}
