using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public Transform firePoint; 
    public Transform target; 
    public float fireRate = 2f;  
    public float bulletSpeed = 10f; 
    public float shootingRange = 10f; 

    [Header("Bullet Pool")]
    public Transform bulletPool;
    public int poolSize = 20;
    private readonly List<Rigidbody> bullets = new List<Rigidbody>();

    private float nextFireTime;

    void Awake()
    {
        // ▬▬▬ ASIGNAR TARGET AUTOMÁTICAMENTE ▬▬▬
        if (target == null)
        {
            GameObject tower = GameObject.FindGameObjectWithTag("Tower");
            if (tower != null)
                target = tower.transform;
            else
                Debug.LogError("EnemyShooting: No se encontró un objeto con el tag 'Tower'");
        }

        // Crear contenedor del pool si no existe
        if (bulletPool == null)
        {
            var poolGo = new GameObject($"{name}_EnemyBulletPool");
            bulletPool = poolGo.transform;
        }

        // Crear balas del pool
        for (int i = 0; i < poolSize; i++)
        {
            var go = Instantiate(bulletPrefab, bulletPool);
            go.SetActive(false);
            var rb = go.GetComponent<Rigidbody>();
            if (rb != null)
                bullets.Add(rb);
        }
    }

    void Update()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= shootingRange && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Shoot()
    {
        var rb = bullets.FirstOrDefault(b => !b.gameObject.activeInHierarchy);
        if (rb == null) return;

        rb.transform.position = firePoint.position;
        rb.transform.rotation = firePoint.rotation;
        rb.gameObject.SetActive(true);

        Vector3 direction = (target.position - firePoint.position).normalized;
        rb.linearVelocity = direction * bulletSpeed;
    }
}
