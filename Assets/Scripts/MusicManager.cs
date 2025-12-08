using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Nombres de escenas")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";   // menú principal
    [SerializeField] private string gameplaySceneName = "SampleScene";  // escena de juego

    [Header("Pistas")]
    public AudioSource menuMusic;      // música del menú
    public AudioSource mainMusic;      // música del gameplay
    public AudioSource gameOverMusic;  // música de game over (canvas)
    public AudioSource victoryMusic;   // música de victoria (canvas)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Manejar también la escena que ya está cargada al iniciar el juego
            Scene current = SceneManager.GetActiveScene();
            OnSceneLoaded(current, LoadSceneMode.Single);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Solo cambiamos música automáticamente cuando cambiamos de ESCENA
        if (scene.name == mainMenuSceneName)
        {
            PlayMenuMusic();
        }
        else if (scene.name == gameplaySceneName)
        {
            PlayMainMusic();
        }
    }

    // --- Helpers ---

    private void StopAllMusic()
    {
        if (menuMusic     != null) menuMusic.Stop();
        if (mainMusic     != null) mainMusic.Stop();
        if (gameOverMusic != null) gameOverMusic.Stop();
        if (victoryMusic  != null) victoryMusic.Stop();
    }

    public void PlayMenuMusic()
    {
        StopAllMusic();
        if (menuMusic != null && !menuMusic.isPlaying)
            menuMusic.Play();
    }

    public void PlayMainMusic()
    {
        StopAllMusic();
        if (mainMusic != null && !mainMusic.isPlaying)
            mainMusic.Play();
    }

    public void PlayGameOverMusic()
    {
        StopAllMusic();
        if (gameOverMusic != null && !gameOverMusic.isPlaying)
            gameOverMusic.Play();
    }

    public void PlayVictoryMusic()
    {
        StopAllMusic();
        if (victoryMusic != null && !victoryMusic.isPlaying)
            victoryMusic.Play();
    }
}