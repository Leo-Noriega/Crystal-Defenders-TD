using System; // Necesario para usar 'Action'
using UnityEngine;

public static class GameEvents
{
    // Evento para cuando cambian las monedas (pasa la cantidad total)
    public static event Action<int> OnMonedasCambian;

    // Evento para cuando cambia la oleada (pasa el numero de oleada)
    public static event Action<int> OnOleadaCambia;

    // Evento para la vida (pasa el porcentaje de 0 a 1)
    public static event Action<float> OnVidaCambia;

    // --- Métodos para "disparar" los eventos ---

    public static void TriggerMonedas(int cantidad)
    {
        OnMonedasCambian?.Invoke(cantidad);
    }

    public static void TriggerOleada(int oleada)
    {
        OnOleadaCambia?.Invoke(oleada);
    }

    public static void TriggerVida(float porcentajeVida)
    {
        OnVidaCambia?.Invoke(porcentajeVida);
    }
}