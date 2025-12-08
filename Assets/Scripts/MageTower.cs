using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class  MageTower : MonoBehaviour
{
    [Header("Refs")]
    public Transform firePoint;              // Ponlo encima del cilindro
    public GameObject bulletPrefab;
    public Transform bulletPool;

    [Header("Config")]
    public float range = 50f;                // Rango de detección
    public LayerMask enemyLayers;
    public float fireRate = 1f;              // Balas por segundo
    [Header("Damage")]
    public float damagePerShot = 25f;

    [Header("Audio")]
    public AudioClip mageAttackSound; // Sonido del disparo de mago
    [Range(0f, 1f)]
    public float attackVolume = 1f;   // Volumen del ataque (0 a 1)

    readonly List<MageBullet> bullets = new();
    float fireCd;
    private AudioSource audioSource;

    void Awake()
    {
        if (!bulletPool) bulletPool = new GameObject($"{name}_BulletPool").transform;

        for (int i = 0; i < 20; i++)
        {
            var go = Instantiate(bulletPrefab, bulletPool);
            var b = go.GetComponent<MageBullet>();
            // Asegúrate de que el prefab tenga el componente SimpleBullet
            if (b != null)
            {
                bullets.Add(b);
                go.SetActive(false);
            }
            else Debug.LogError("¡Tu prefab de bala no tiene el script SimpleBullet!");
        }

        // Inicializar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
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
        // Reproducir sonido de disparo de mago
        if (audioSource != null && mageAttackSound != null)
        {
            audioSource.PlayOneShot(mageAttackSound, attackVolume);
        }

        var bullet = bullets.FirstOrDefault(x => !x.gameObject.activeInHierarchy);
        if (bullet)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation; // Sale con la orientación del firepoint
            bullet.gameObject.SetActive(true);
            bullet.SetTarget(target, damagePerShot);
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