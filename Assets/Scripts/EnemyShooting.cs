using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Assign the Bullet prefab here
    public Transform firePoint; // Assign the point from where the bullet will be fired
    public Transform target; // Assign the TowerTag1 GameObject here

    [Header("Auto Targeting")] 
    public string targetTag = "Tower"; // Tag que usará para encontrar la torre

    public float fireRate = 2f; // Time between shots
    public float bulletSpeed = 10f; // Speed of the bullet
    public float shootingRange = 10f; // Distance within which the enemy starts shooting

    [Header("Bullet Pool")]
    public Transform bulletPool;         // Optional parent for pooled bullets
    public int poolSize = 20;            // Max bullets this enemy can have alive
    private readonly List<Rigidbody> bullets = new List<Rigidbody>();

    private float nextFireTime;

    void Awake()
    {
        // Desactivado - los enemigos ahora usan ataque cuerpo a cuerpo (EnemyAttack)
        return;
    }

    void Update()
    {
        // Desactivado - los enemigos ahora usan ataque cuerpo a cuerpo (EnemyAttack)
        return;
    }

    void Shoot()
    {
        // Recoger una bala inactiva del pool
        var rb = bullets.FirstOrDefault(b => !b.gameObject.activeInHierarchy);
        if (rb == null)
        {
            // No hay balas disponibles en el pool, no disparamos
            return;
        }

        // Posicionar y activar la bala
        rb.transform.position = firePoint.position;
        rb.transform.rotation = firePoint.rotation;
        rb.gameObject.SetActive(true);

        // Darle velocidad hacia el objetivo
        if (target != null)
        {
            Vector3 direction = (target.position - firePoint.position).normalized;
            rb.linearVelocity = direction * bulletSpeed;
        }
        else
        {
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }
    }
}