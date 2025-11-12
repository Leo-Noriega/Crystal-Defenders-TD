using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
 
public class TowerHealth : MonoBehaviour
{
    [Header("Game Over UI")] 
    [SerializeField] private GameObject gameOverPanel; // Panel con el mensaje GAME OVER (inactivo al inicio)
    [SerializeField] private Button retryButton;       // Botón "Jugar de nuevo"
    [SerializeField] private AudioSource gameOverMusic;

    public float maxHealth = 100f; // Maximum health of the tower
    private float currentHealth;

    public string towerName; // Unique name or ID for the tower
    [SerializeField] private Slider healthSlider; // Assign via Inspector or at runtime

    void Start()
    {
        // Initialize the health
        currentHealth = maxHealth;

        // Registrar referencias UI si esta torre las tiene asignadas
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Find the corresponding health bar in the UI
        // Expect the slider to be assigned in the Inspector or via SetHealthBar()
        if (healthSlider == null)
        {
            Debug.LogError($"[TowerHealth] No healthSlider assigned on {name}. Assign it in the Inspector or call SetHealthBar().");
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
        // Destruir visualmente la torre
        Destroy(gameObject);

        // Mostrar pantalla de Game Over ya que esta es la única torre
        TriggerGameOver();
    }

    private void TriggerGameOver()
    {
        // Pausar el juego (opcional); comenta esta línea si no quieres pausar
        Time.timeScale = 0f;

        if (gameOverMusic != null)
        {
            gameOverMusic.Play();
        }
        else
        {
            Debug.LogWarning("No se asignó música de Game Over.");
        }
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (retryButton != null)
            {
                retryButton.onClick.RemoveAllListeners();
                retryButton.onClick.AddListener(() =>
                {
                    Time.timeScale = 1f;
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