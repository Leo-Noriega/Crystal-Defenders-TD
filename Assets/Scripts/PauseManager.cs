using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    // --- Singleton ---
    public static PauseManager instance;
    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
    }
    // -----------------

    [Header("UI References")]
    public GameObject pauseMenuPanel; // Panel del menú de pausa

    [Header("Input")]
    public InputActionReference pauseAction; // Referencia a la acción de pausa del InputSystem

    private bool isPaused = false;

    void Start()
    {
        // Asegurarse de que el menú esté oculto al inicio
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        // Suscribirse a la acción de pausa si está asignada
        if (pauseAction != null)
        {
            pauseAction.action.performed += OnPauseInput;
            pauseAction.action.Enable();
        }
    }

    void OnDestroy()
    {
        // Desuscribirse al destruir el objeto
        if (pauseAction != null)
        {
            pauseAction.action.performed -= OnPauseInput;
        }
    }

    void Update()
    {
        // Fallback: Si no se usa InputSystem, detectar ESC manualmente
        if (pauseAction == null && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Callback del InputSystem
    private void OnPauseInput(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    // Alternar entre pausado y no pausado
    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    // Pausar el juego
    public void Pause()
    {
        // No permitir pausar si ya estamos en game over o victoria
        // (puedes ajustar esta lógica según tu necesidad)
        if (Time.timeScale == 0f && !isPaused)
        {
            // Ya está pausado por otra razón (game over/victory)
            return;
        }

        isPaused = true;
        Time.timeScale = 0f; // Detener el tiempo del juego
        AudioListener.pause = true; // Pausar todo el audio

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
    }

    // Reanudar el juego
    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f; // Restaurar el tiempo normal
        AudioListener.pause = false; // Reanudar el audio

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
    }

    // Volver al menú principal (llamado por botón de UI)
    public void ReturnToMainMenu()
    {
        // Asegurarse de restaurar el tiempo antes de cambiar de escena
        Time.timeScale = 1f;
        AudioListener.pause = false;
        isPaused = false;

        SceneManager.LoadScene("MainMenu");
    }

    // Propiedad pública para verificar si está pausado
    public bool IsPaused => isPaused;
}
