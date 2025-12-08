using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;    // Prefab del enemigo
    public Transform spawnPoint;      // Lugar donde aparecen

    [Header("Wave Settings")]
    public int[] enemiesPerWave = { 3, 5, 7 }; // Enemigos por ronda
    public float timeBetweenWaves = 5f;        // Tiempo entre rondas

    private int currentWave = 0;
    private List<GameObject> currentEnemies = new List<GameObject>();
    private bool isWaitingNextWave = false;
    
    [Header("Victory UI")]
    [SerializeField] private GameObject victoryPanel;      // Panel de victoria (inactivo al inicio)
    [SerializeField] private Button victoryRetryButton;    // Botón "Jugar de nuevo" (opcional)
    [SerializeField] private Button victoryMenuButton; // Botón para ir al menú principal

    private bool hasWon = false;

    void Start()
    {
        StartCoroutine(StartNextWave());
        // Asegurar que el panel de victoria está apagado al inicio
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(1f);
        SpawnWave(currentWave);
    }

    void SpawnWave(int waveIndex)
    {
        Debug.Log($"[EnemyManager] Iniciando ronda {waveIndex + 1}");
        GameEvents.TriggerOleada(waveIndex + 1);
        int amount = enemiesPerWave[waveIndex];

        currentEnemies.Clear();

        for (int i = 0; i < amount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            currentEnemies.Add(enemy);

            // Si tus enemigos tienen un script EnemyHealth, registra su muerte
            EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                eh.onEnemyDeath += OnEnemyDeath;
            }
        }
    }

    void OnEnemyDeath(GameObject enemy)
    {
        if (currentEnemies.Contains(enemy))
            currentEnemies.Remove(enemy);

        // ¿Ya murieron todos?
        if (currentEnemies.Count == 0)
        {
            Debug.Log("[EnemyManager] Todos los enemigos de la ronda murieron.");

            currentWave++;

            if (currentWave < enemiesPerWave.Length)
            {
                StartCoroutine(WaitAndStartNextWave());
            }
            else
            {
                isWaitingNextWave = true;
                TriggerVictory();
            }
        }
    }

    IEnumerator WaitAndStartNextWave()
    {
        Debug.Log($"[EnemyManager] Esperando {timeBetweenWaves} segundos para la siguiente ronda...");
        yield return new WaitForSeconds(timeBetweenWaves);

        SpawnWave(currentWave);
    }

private void TriggerVictory()
{
    if (hasWon)
        return; // evitar llamarlo dos veces

    hasWon = true;

    // Cambiar música a victoria
    var musicManager = MusicManager.Instance ?? FindFirstObjectByType<MusicManager>();
    if (musicManager != null)
    {
        musicManager.PlayVictoryMusic();
    }

    // Pausar el juego al ganar (opcional)
    Time.timeScale = 0f;

    if (victoryPanel != null)
    {
        victoryPanel.SetActive(true);

        if (victoryRetryButton != null)
        {
            victoryRetryButton.onClick.RemoveAllListeners();
            victoryRetryButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1f; // reanudar

                // Volver a la música principal del gameplay
                var mm = MusicManager.Instance ?? FindFirstObjectByType<MusicManager>();
                if (mm != null)
                {
                    mm.PlayMainMusic();
                }

                Scene current = SceneManager.GetActiveScene();
                SceneManager.LoadScene(current.buildIndex);
            });
        }
        else
        {
            Debug.LogWarning("[EnemyManager] victoryRetryButton no asignado.");
        }

        if (victoryMenuButton != null)
        {
            victoryMenuButton.onClick.RemoveAllListeners();
            victoryMenuButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;

                // Reproducir música del menú principal
                var mm = MusicManager.Instance ?? FindFirstObjectByType<MusicManager>();
                if (mm != null)
                {
                    mm.PlayMenuMusic();
                }

                SceneManager.LoadScene("MainMenu"); // Ajusta según el nombre real del menú
            });
        }
        else
        {
            Debug.LogWarning("[EnemyManager] victoryMenuButton no asignado.");
        }
    }
    else
    {
        Debug.LogWarning("[EnemyManager] victoryPanel no asignado.");
    }
}
}