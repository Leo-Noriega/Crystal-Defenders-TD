using UnityEngine;

public class GameOverMusicController : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        // Evita que se destruya al cambiar de escena
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No se encontró un AudioSource en GameOverMusic. Agrega uno en el Inspector.");
        }
    }

    public void PlayMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}