using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Cambiar de música: de menú → gameplay
        var musicManager = MusicManager.Instance ?? FindFirstObjectByType<MusicManager>();
        if (musicManager != null)
        {
            musicManager.PlayMainMusic();   // esto detiene la del menú y pone la principal
        }

        // Cargar la escena del juego
        SceneManager.LoadScene("SampleScene");
    }
}