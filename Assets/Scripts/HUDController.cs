using UnityEngine;
using UnityEngine.UI;
using TMPro; // Importante para tus textos

public class HUDController : MonoBehaviour
{
    [Header("Referencias (Arrastra desde tu Jerarquía)")]
    [SerializeField] private Slider sliderVida;
    [SerializeField] private TextMeshProUGUI textoMonedas; // CoinText
    [SerializeField] private TextMeshProUGUI textoOleadas; // WaveText

    [Header("Configuración Visual")]
    [SerializeField] private float velocidadBarra = 5f;
    
    private float _vidaObjetivo = 1f; // Para la animación suave

    // 1. Nos suscribimos a los eventos al activar el objeto
    private void OnEnable()
    {
        GameEvents.OnMonedasCambian += ActualizarMonedas;
        GameEvents.OnOleadaCambia += ActualizarOleada;
        GameEvents.OnVidaCambia += ActualizarVida;
    }

    // 2. Nos des-suscribimos al desactivar (Muy importante para evitar errores de memoria)
    private void OnDisable()
    {
        GameEvents.OnMonedasCambian -= ActualizarMonedas;
        GameEvents.OnOleadaCambia -= ActualizarOleada;
        GameEvents.OnVidaCambia -= ActualizarVida;
    }

    private void Update()
    {
        // Animación suave de la barra de vida (Lerp)
        if (sliderVida.value != _vidaObjetivo)
        {
            sliderVida.value = Mathf.Lerp(sliderVida.value, _vidaObjetivo, Time.deltaTime * velocidadBarra);
        }
    }

    // --- Las funciones que responden a los eventos ---

    private void ActualizarMonedas(int cantidad)
    {
        // Formato "000" para que se vea estético (ej: 005, 012, 100)
        textoMonedas.text = "x " + cantidad.ToString("D3"); 
    }

    private void ActualizarOleada(int oleada)
    {
        textoOleadas.text = "WAVE " + oleada.ToString();
    }

    private void ActualizarVida(float porcentaje)
    {
        _vidaObjetivo = porcentaje;
        
        // Opcional: Cambiar color si la vida es baja
        // if(porcentaje < 0.3f) ... cambiar color del Fill a rojo
    }
}