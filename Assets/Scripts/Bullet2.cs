using UnityEngine;

public class Bullet2 : MonoBehaviour
{
    public float damage = 10f; // Damage dealt by the bullet

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hits a tower
        TowerHealth towerHealth = collision.gameObject.GetComponent<TowerHealth>();
        if (towerHealth != null)
        {
            // Apply damage to the tower
            towerHealth.TakeDamage(damage);

            // Destroy the bullet after it hits the tower
            Destroy(gameObject);
        }
    }
}