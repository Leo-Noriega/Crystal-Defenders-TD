using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Assign the Bullet prefab here
    public Transform firePoint; // Assign the point from where the bullet will be fired
    public Transform target; // Assign the TowerTag1 GameObject here
    public float fireRate = 2f; // Time between shots
    public float bulletSpeed = 10f; // Speed of the bullet
    public float shootingRange = 10f; // Distance within which the enemy starts shooting

    private float nextFireTime;

    void Update()
    {
        if (target != null)
        {
            // Calculate the distance between the enemy and the target
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // Check if the enemy is within shooting range and it's time to shoot
            if (distanceToTarget <= shootingRange && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate; // Set the next fire time
            }
        }
    }

    void Shoot()
    {
        // Instantiate the bullet prefab at the fire point's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Add velocity to the bullet to make it move towards the target
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (target.position - firePoint.position).normalized;
            rb.linearVelocity = direction * bulletSpeed;
        }
    }
}