using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject enemy_container;
    public float upBound = 9.25f;
    public float leftBound = -10f;
    public float rightBound = 10f;
    private bool stopSpawning = false;
   

    IEnumerator SpawnRoutine()
    {
        while (!stopSpawning)
        {
            float xRange = Random.Range(leftBound + 1, rightBound - 1);
            GameObject newEnemy = Instantiate(enemy, new Vector3(xRange, upBound, 0), Quaternion.identity);
            newEnemy.transform.parent = enemy_container.transform;
            yield return new WaitForSeconds(1);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
        Player.playerDeath += onPlayerDeath;
    }

    public void onPlayerDeath()
    {
        stopSpawning = true;
    }
}
