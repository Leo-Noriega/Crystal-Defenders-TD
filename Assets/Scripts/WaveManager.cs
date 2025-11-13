using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject enemyPrefab;       // Prefab del Enemy
    public Transform spawnPoint;         // Punto de spawn

    [Header("Waves")]
    public int startingEnemies = 2;      // Enemigos de la primera oleada
    public int enemyIncrement = 2;       // Cuántos enemigos adicionales por oleada
    public float spawnInterval = 2f;     // Tiempo entre enemigos
    public float timeBetweenWaves = 15f; // Tiempo entre oleadas
    public int totalWaves = 50;          // Total de oleadas

    [Header("UI (opcional)")]
    public Text waveText;                // Asignar en Inspector (UI Text)
    public Text enemiesKilledText;        // Asignar en Inspector (UI Text)

    private int currentWave = 0;
    [HideInInspector] public int enemiesDeath = 0; 

    void Start()
    {
        UpdateUI();
        StartCoroutine(StartWaves());
    }

    private IEnumerator StartWaves()
    {
        int enemiesInWave = startingEnemies;

        while (currentWave < totalWaves)
        {
            // Aumentamos la wave antes de spawnear para que el contador muestre la wave "1" desde la primera oleada
            currentWave++;

            for (int i = 0; i < enemiesInWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }

            enemiesInWave += enemyIncrement; // Aumenta la cantidad para la siguiente oleada

            UpdateUI();
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        // Fin de oleadas: opcionalmente puedes mostrar algo aquí
        UpdateUI();
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab no asignado en WaveManager!");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Spawn Point no asignado en WaveManager!");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        UpdateUI();

        // Asignar el reporter que avisará cuando este GameObject sea destruido
        // Si el prefab ya trae el componente, se reutiliza; si no, se añade en runtime.
        EnemyDeathReporter reporter = enemy.GetComponent<EnemyDeathReporter>();
        if (reporter == null)
        {
            reporter = enemy.AddComponent<EnemyDeathReporter>();
        }
        reporter.manager = this;
    }

    // Método público llamado por EnemyDeathReporter en OnDestroy
    public void ReportEnemyDeath()
    {
        enemiesDeath++;
        if (enemiesDeath < 0) enemiesDeath = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave: {currentWave}";
        }

        if (enemiesKilledText != null)
        {
            enemiesKilledText.text = $"Kills: {enemiesDeath}";
        }
    }
}
