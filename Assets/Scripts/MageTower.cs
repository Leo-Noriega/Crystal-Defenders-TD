using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleTurret : MonoBehaviour
{
    [Header("Refs")]
    public Transform firePoint;              // Ponlo encima del cilindro
    public GameObject bulletPrefab;
    public Transform bulletPool;

    [Header("Config")]
    public float range = 15f;                // Rango de detección
    public LayerMask enemyLayers;
    public float fireRate = 1f;              // Balas por segundo

    readonly List<SimpleBullet> bullets = new();
    float fireCd;

    void Awake()
    {
        if (!bulletPool) bulletPool = new GameObject($"{name}_BulletPool").transform;

        for (int i = 0; i < 20; i++)
        {
            var go = Instantiate(bulletPrefab, bulletPool);
            var b = go.GetComponent<SimpleBullet>();
            // Asegúrate de que el prefab tenga el componente SimpleBullet
            if (b != null)
            {
                bullets.Add(b);
                go.SetActive(false);
            }
            else Debug.LogError("¡Tu prefab de bala no tiene el script SimpleBullet!");
        }
    }

    void Update()
    {
        fireCd -= Time.deltaTime;
        if (fireCd <= 0f)
        {
            // Buscamos objetivo justo antes de disparar
            Transform target = AcquireTarget();
            if (target != null)
            {
                Fire(target);
                fireCd = 1f / fireRate;
            }
        }
    }

    void Fire(Transform target)
    {
        var bullet = bullets.FirstOrDefault(x => !x.gameObject.activeInHierarchy);
        if (bullet)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation; // Sale con la orientación del firepoint
            bullet.gameObject.SetActive(true);
            // ¡Aquí está la magia! Le decimos a la bala quién es su presa.
            bullet.SetTarget(target);
        }
    }

    Transform AcquireTarget()
    {
        // Busca todos los enemigos en rango
        var hits = Physics.OverlapSphere(transform.position, range, enemyLayers);
        if (hits.Length == 0) return null;

        // Devuelve el primero que encuentre activo (puedes mejorar esto para que busque el más cercano si quieres)
        foreach (var h in hits)
        {
            if (h.gameObject.activeInHierarchy) return h.transform;
        }
        return null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}