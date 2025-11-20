using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        
        var musicController = Object.FindFirstObjectByType<GameOverMusicController>();
        if (musicController != null)
        {
            musicController.StopMusic();
        }
        SceneManager.LoadScene("HomeScreen");
    }
}
