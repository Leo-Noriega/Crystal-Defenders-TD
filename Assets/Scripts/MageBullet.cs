using UnityEngine;

public class MageBullet : MonoBehaviour
{
    [Header("Config Bala")]
    public float speed = 5f;
    public float turnSpeed = 5f;     // Qué tan rápido gira para perseguir al enemigo
    public float lifeSeconds = 5f;
    public float hitDistance = 0.3f;

    private Transform target;
    private float life;
    private float damage;

    void OnEnable()
    {
        life = lifeSeconds;
    }

    // La torreta llamará a esto al disparar
    public void SetTarget(Transform newTarget, float newDamage)
    {
        target = newTarget;
        damage = newDamage;
    }

    void Update()
        {
            // Si ya no hay objetivo válido, devolver la bala al pool
            if (target == null || !target.gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
                return;
            }
    
            // 1. Dirección hacia el enemigo
            Vector3 dirToTarget = (target.position - transform.position).normalized;
    
            // Rotamos suavemente la bala para mirar al enemigo
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dirToTarget),
                Time.deltaTime * turnSpeed
            );
    
            // 2. Movernos en dirección al objetivo
            transform.position += dirToTarget * speed * Time.deltaTime;
    
            // 3. Comprobar si ya "pegamos"
            float dist = Vector3.Distance(transform.position, target.position);
            if (dist <= hitDistance)
            {
                // Intentar hacer daño usando EnemyHealth
                var health = target.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                    Debug.Log($"[MageBullet] Golpeó a {target.name} por {damage} de daño.");
                }
                else
                {
                    Debug.LogWarning($"[MageBullet] El objetivo {target.name} no tiene EnemyHealth.");
                }
    
                // Devolver al pool
                gameObject.SetActive(false);
                return;
            }
    
            // 4. Autodestrucción por tiempo (seguro por si algo falla)
            life -= Time.deltaTime;
            if (life <= 0f)
            {
                gameObject.SetActive(false);
            }
        }

}