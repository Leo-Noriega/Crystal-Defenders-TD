using UnityEngine;

public class MortarBullet : MonoBehaviour
{
[Header("Movimiento")]
    public float speed = 15f;        // Velocidad base del viaje
    public float arcHeight = 5f;     // Altura máxima
    [Range(0.1f, 0.9f)]
    public float peakPoint = 0.4f;   // Dónde ocurre la altura máxima (0.5 = mitad, <0.5 = sube rápido y cae lento, >0.5 = sube lento y cae rápido)

    [Header("Daño")]
    public float damageRadius = 3f;
    public LayerMask enemyLayers;    // Para optimizar la detección de enemigos

    private Vector3 startPos;
    private Vector3 targetPos;
    private float progress;

    public void Launch(Vector3 target)
    {
        startPos = transform.position;
        targetPos = target;
        progress = 0f;
        gameObject.SetActive(true);
    }

    void Update()
    {
        float distance = Vector3.Distance(startPos, targetPos);
        if (distance < 0.1f) distance = 0.1f;

        // Avanzamos el progreso
        progress += (Time.deltaTime * speed) / distance;

        if (progress >= 1.0f)
        {
            Impact();
            return;
        }

        // --- NUEVA MAGIA MATEMÁTICA (Curva Bezier Cuadrática modificada) ---
        Vector3 currentPos = Vector3.Lerp(startPos, targetPos, progress);

        // Calculamos la altura usando una curva asimétrica
        // Si progress < peakPoint, estamos subiendo. Si progress > peakPoint, estamos bajando.
        float height;
        if (progress < peakPoint)
        {
            // Fase de subida (más lenta si peakPoint es alto)
            float t = progress / peakPoint;
            // Usamos una función suave (seno) para la subida
            height = Mathf.Sin(t * Mathf.PI * 0.5f) * arcHeight;
        }
        else
        {
            // Fase de bajada (más rápida si peakPoint es alto)
            float t = (progress - peakPoint) / (1f - peakPoint);
            // Usamos coseno invertido para la bajada, da un efecto de caída acelerada
            height = Mathf.Cos(t * Mathf.PI * 0.5f) * arcHeight;
        }

        currentPos.y += height;
        transform.position = currentPos;

        // Rotación opcional para mirar hacia donde va (calculando la siguiente posición)
        // Esto puede fallar un poco en el pico, pero generalmente funciona bien.
        Vector3 nextPosGuess = Vector3.Lerp(startPos, targetPos, progress + 0.01f);
        float nextHeight;
        float nextProg = progress + 0.01f;
         if (nextProg < peakPoint)
        {
             float t = nextProg / peakPoint;
             nextHeight = Mathf.Sin(t * Mathf.PI * 0.5f) * arcHeight;
        }
        else
        {
             float t = (nextProg - peakPoint) / (1f - peakPoint);
             nextHeight = Mathf.Cos(t * Mathf.PI * 0.5f) * arcHeight;
        }
        nextPosGuess.y += nextHeight;
        transform.LookAt(nextPosGuess);
    }

    void Impact()
    {
        // Lógica de daño optimizada con LayerMask
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, damageRadius, enemyLayers);
        foreach (Collider enemy in hitEnemies)
        {
             // Tu lógica de daño aquí, ej: enemy.GetComponent<Enemy>().TakeDamage(10);
             Debug.Log("BOOM! " + enemy.name);
        }
        gameObject.SetActive(false);
    }
}
