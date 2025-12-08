using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
 
public class TowerHealth : MonoBehaviour
{
    [Header("Game Over UI")] 
    [SerializeField] private GameObject gameOverPanel; // Panel con el mensaje GAME OVER (inactivo al inicio)
    [SerializeField] private Button retryButton;       // Botón "Jugar de nuevo"

    public float maxHealth = 100f; // Maximum health of the tower
    public float currentHealth;

    public string towerName; // Unique name or ID for the tower
    [SerializeField] private Slider healthSlider; // Assign via Inspector or at runtime

    [Header("Audio")]
    public AudioClip damageSound;     // Sonido cuando la torre recibe daño
    [Range(0f, 1f)]
    public float damageVolume = 1f;   // Volumen del sonido de daño
    private AudioSource audioSource;

    void Start()
    {
        // Initialize the health
        currentHealth = maxHealth;
        GameEvents.TriggerVida(1f);

        // Inicializar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Registrar referencias UI si esta torre las tiene asignadas
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Find the corresponding health bar in the UI
        // Expect the slider to be assigned in the Inspector or via SetHealthBar()
        if (healthSlider == null)
        {
            //Debug.LogError($"[TowerHealth] No healthSlider assigned on {name}. Assign it in the Inspector or call SetHealthBar().");
            return;
        }

        // Set the slider's max value and current value
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        // Reduce health by the damage amount
        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        float porcentageLife = currentHealth / maxHealth;
        GameEvents.TriggerVida(porcentageLife);

        // Reproducir sonido de daño
        if (audioSource != null && damageSound != null && currentHealth > 0)
        {
            audioSource.PlayOneShot(damageSound, damageVolume);
        }

        // Update the health slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Check if the tower's health is depleted
        if (currentHealth <= 0)
        {
            DestroyTower();
        }
    }

    void DestroyTower()
    {
        var musicManager = MusicManager.Instance ?? FindFirstObjectByType<MusicManager>();
        if (musicManager != null)
        {
            musicManager.PlayGameOverMusic();
        }

        Destroy(gameObject);
        TriggerGameOver();
    }
    private void TriggerGameOver()
    {
        // Pausar el juego (opcional); comenta esta línea si no quieres pausar
        Time.timeScale = 0f;

        // Mostrar panel de Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (retryButton != null)
            {
                retryButton.onClick.RemoveAllListeners();
                retryButton.onClick.AddListener(() =>
                {
                    // Reanudar el tiempo
                    Time.timeScale = 1f;

                    // Volver a la música principal
                    var musicManager = MusicManager.Instance ?? FindFirstObjectByType<MusicManager>();
                    if (musicManager != null)
                    {
                        musicManager.PlayMainMusic();
                    }

                    // Recargar la escena actual
                    Scene current = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(current.buildIndex);
                });
            }
            else
            {
                Debug.LogWarning("[TowerHealth] retryButton no asignado.");
            }
        }
        else
        {
            Debug.LogError("[TowerHealth] gameOverPanel no asignado.");
        }
    }

    public void SetHealthBar(Slider slider)
    {
        healthSlider = slider;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
            
        }
    }
}