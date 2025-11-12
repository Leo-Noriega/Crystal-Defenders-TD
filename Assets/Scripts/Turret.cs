// csharp
        // File: `Assets/Scripts/Turret.cs`
        using System.Collections;
        using System.Collections.Generic;
        using System.Linq;
        using UnityEngine;
        
        public class Turret : MonoBehaviour
        {
            public GameObject bulletPrefab;
            public GameObject firePoint;
            public GameObject bulletPool;
            private List<Bullet> bullets = new();
            [SerializeField] private float bulletSpeed = 20f;
            [SerializeField] private int poolSize = 50;
            [SerializeField] private float fireInterval = 0.5f;
        
            private void Awake()
            {
                if (bulletPool == null)
                {
                    Debug.LogError("Asignar `bulletPool` en el Inspector.");
                    return;
                }
        
                if (bulletPrefab == null)
                {
                    Debug.LogError("Asignar `bulletPrefab` en el Inspector.");
                    return;
                }
        
                for (int i = 0; i < poolSize; i++)
                {
                    var instance = Instantiate(bulletPrefab, bulletPool.transform);
                    var bullet = instance.GetComponent<Bullet>();
                    if (bullet == null) bullet = instance.AddComponent<Bullet>();
                    // opcional: asegurarse de que el prefab tenga DamageProjectile para aplicar daño
                    if (instance.GetComponent<DamageProjectile>() == null)
                    {
                        instance.AddComponent<DamageProjectile>();
                    }
                    instance.SetActive(false);
                    bullets.Add(bullet);
                }
            }
        
            IEnumerator Start()
            {
                while (true)
                {
                    var available = bullets.FirstOrDefault(x => !x.gameObject.activeInHierarchy);
                    if (available != null && firePoint != null)
                    {
                        available.direction = firePoint.transform.up;
                        available.transform.position = firePoint.transform.position;
                        available.gameObject.SetActive(true);
        
                        Rigidbody rb = available.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.linearVelocity = available.direction.normalized * bulletSpeed;
                        }
                    }
                    yield return new WaitForSeconds(fireInterval);
                }
            }
        }