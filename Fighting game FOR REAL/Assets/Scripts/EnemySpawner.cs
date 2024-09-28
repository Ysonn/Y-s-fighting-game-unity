using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your enemy prefab in the Inspector
    public float initialSpawnRate = 5f; // Initial spawn rate in seconds
    public float decreaseRate = 0.1f; // Rate to decrease spawn time (in seconds)
    public float maxSpawnRate = 2f; // Maximum spawn rate (in seconds)
    public float timeToMaxRate = 60f; // Time to reach max spawn rate (in seconds)

    private float currentSpawnRate;
    private float elapsedTime;

    void Start()
    {
        currentSpawnRate = initialSpawnRate;
        elapsedTime = 0f;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnRate);
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            elapsedTime += currentSpawnRate;

            // Increase spawn rate as time passes
            if (elapsedTime < timeToMaxRate)
            {
                currentSpawnRate = Mathf.Max(maxSpawnRate, initialSpawnRate - (decreaseRate * (elapsedTime / timeToMaxRate)));
            }
            else
            {
                currentSpawnRate = maxSpawnRate; // Maintain max spawn rate
            }
        }
    }
}
