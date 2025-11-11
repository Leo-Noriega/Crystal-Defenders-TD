using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    [Header("Config Bala")]
    public float speed = 5f;         // Velocidad lenta como pediste
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
        // 1. Si tiene objetivo vivo, ajusta la dirección hacia él
        if (target != null && target.gameObject.activeInHierarchy)
        {
            // Dirección hacia el enemigo
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            // Rotamos suavemente la bala para mirar al enemigo
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dirToTarget), Time.deltaTime * turnSpeed);
        }

        // 2. Moverse siempre hacia adelante (su propio "frente")
        transform.position += transform.forward * speed * Time.deltaTime;

        // 3. Autodestrucción por tiempo
        life -= Time.deltaTime;
        if (life <= 0f) gameObject.SetActive(false);
    }

    // Opcional: Desactivar si choca con algo
    void OnTriggerEnter(Collider other)
    {
        // Aquí puedes poner tu lógica de daño
        // if (other.CompareTag("Enemy")) { ... }
        gameObject.SetActive(false);
    }
}