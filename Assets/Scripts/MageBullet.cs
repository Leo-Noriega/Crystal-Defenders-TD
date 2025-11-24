using UnityEngine;

public class MageBullet : MonoBehaviour
{
    [Header("Config Bala")]
    public float speed = 5f;
    public float turnSpeed = 5f;     // Qué tan rápido gira para perseguir al enemigo
    public float lifeSeconds = 5f;

    private Transform target;
    private float life;

    void OnEnable()
    {
        life = lifeSeconds;
    }

    // La torreta llamará a esto al disparar
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        // 1. Si tiene objetivo vivo y activo, perseguimos
        if (target != null && target.gameObject.activeInHierarchy)
        {
            // Dirección hacia el enemigo
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            // Rotamos suavemente la bala para mirar al enemigo
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dirToTarget),
                Time.deltaTime * turnSpeed
            );

            // 2. Movernos en dirección al objetivo (no solo hacia forward)
            transform.position += dirToTarget * speed * Time.deltaTime;
        }
        else
        {
            // Si ya no hay objetivo válido, devolver la bala al pool
            gameObject.SetActive(false);
            return;
        }

        // 3. Autodestrucción por tiempo (seguro por si algo falla)
        life -= Time.deltaTime;
        if (life <= 0f)
            gameObject.SetActive(false);
    }

}