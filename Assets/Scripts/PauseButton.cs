using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script simple para conectar botones con el PauseManager
/// Añade este script a cada botón de pausa
/// </summary>
[RequireComponent(typeof(Button))]
public class PauseButton : MonoBehaviour
{
    public enum ButtonAction
    {
        Pause,      // Pausar el juego
        Resume,     // Reanudar el juego
        BackToMenu  // Volver al menú principal
    }

    [Header("Configuración")]
    [Tooltip("Acción que ejecutará este botón")]
    public ButtonAction action = ButtonAction.Pause;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        // Verificar que existe el PauseManager
        if (PauseManager.instance == null)
        {
            Debug.LogError($"PauseButton en {gameObject.name}: No se encontró PauseManager en la escena!", this);
            return;
        }

        // Conectar el botón con la acción correspondiente
        switch (action)
        {
            case ButtonAction.Pause:
                button.onClick.AddListener(PauseManager.instance.Pause);
                break;
            case ButtonAction.Resume:
                button.onClick.AddListener(PauseManager.instance.Resume);
                break;
            case ButtonAction.BackToMenu:
                button.onClick.AddListener(PauseManager.instance.ReturnToMainMenu);
                break;
        }
    }

    void OnDestroy()
    {
        // Limpiar los listeners
        if (PauseManager.instance != null && button != null)
        {
            switch (action)
            {
                case ButtonAction.Pause:
                    button.onClick.RemoveListener(PauseManager.instance.Pause);
                    break;
                case ButtonAction.Resume:
                    button.onClick.RemoveListener(PauseManager.instance.Resume);
                    break;
                case ButtonAction.BackToMenu:
                    button.onClick.RemoveListener(PauseManager.instance.ReturnToMainMenu);
                    break;
            }
        }
    }
}
