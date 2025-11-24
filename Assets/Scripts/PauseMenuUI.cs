using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script opcional para manejar elementos adicionales de UI en el menú de pausa
/// como sliders de volumen, opciones gráficas, etc.
/// </summary>
public class PauseMenuUI : MonoBehaviour
{
    [Header("Audio Controls (Optional)")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Display Texts (Optional)")]
    public TextMeshProUGUI masterVolumeText;
    public TextMeshProUGUI musicVolumeText;
    public TextMeshProUGUI sfxVolumeText;

    void Start()
    {
        // Cargar valores guardados de volumen
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            UpdateMasterVolumeText(masterVolumeSlider.value);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            UpdateMusicVolumeText(musicVolumeSlider.value);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            UpdateSFXVolumeText(sfxVolumeSlider.value);
        }
    }

    // Callbacks de los sliders
    private void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
        UpdateMasterVolumeText(value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        // Aquí puedes controlar el volumen de la música específicamente
        // Por ejemplo, si tienes un AudioSource para música:
        // musicAudioSource.volume = value;

        PlayerPrefs.SetFloat("MusicVolume", value);
        UpdateMusicVolumeText(value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        // Aquí puedes controlar el volumen de los efectos de sonido
        // Por ejemplo, usar un AudioMixer

        PlayerPrefs.SetFloat("SFXVolume", value);
        UpdateSFXVolumeText(value);
    }

    // Actualizar textos de UI
    private void UpdateMasterVolumeText(float value)
    {
        if (masterVolumeText != null)
        {
            masterVolumeText.text = $"Master: {Mathf.RoundToInt(value * 100)}%";
        }
    }

    private void UpdateMusicVolumeText(float value)
    {
        if (musicVolumeText != null)
        {
            musicVolumeText.text = $"Music: {Mathf.RoundToInt(value * 100)}%";
        }
    }

    private void UpdateSFXVolumeText(float value)
    {
        if (sfxVolumeText != null)
        {
            sfxVolumeText.text = $"SFX: {Mathf.RoundToInt(value * 100)}%";
        }
    }

    void OnDestroy()
    {
        // Guardar todas las preferencias
        PlayerPrefs.Save();

        // Desuscribirse de los eventos
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);

        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
    }
}
