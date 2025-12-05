using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;       // Prefab del Enemy
    public Transform spawnPoint;         // Punto de spawn
    public int startingEnemies = 2;      // Enemigos de la primera oleada
    public int enemyIncrement = 2;       // Cuántos enemigos adicionales por oleada
    public float spawnInterval = 2f;     // Tiempo entre enemigos
    public float timeBetweenWaves = 15f;  // Tiempo entre oleadas
    public int totalWaves = 5;           // Total de oleadas

    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(StartWaves());
    }

    private IEnumerator StartWaves()
    {
        int enemiesInWave = startingEnemies;

        while (currentWave < totalWaves)
        {

            for (int i = 0; i < enemiesInWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }

            currentWave++;
            enemiesInWave += enemyIncrement; // Aumenta la cantidad para la siguiente oleada


            yield return new WaitForSeconds(timeBetweenWaves);
        }

    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}