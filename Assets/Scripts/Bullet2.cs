using UnityEngine;

public class Bullet2 : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        TowerHealth towerHealth = other.GetComponent<TowerHealth>();
        if (towerHealth != null)
        {
            // Aplicar daño
            towerHealth.TakeDamage(damage);

            // Destruir la bala al impactar
            Destroy(gameObject);
        }
    }
}