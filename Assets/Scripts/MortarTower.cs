using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class MortarTower : MonoBehaviour
{
    [Header("Refs")]
    public Transform firePoint;
    [FormerlySerializedAs("mortarPrefab")] public GameObject mortarBullet;  // Prefab con el script MortarProjectile
    public Transform bulletPool;

    [Header("Config")]
    public float range = 20f;
    public float fireRate = 0.5f;    // Dispara más lento que la torre mágica
    public LayerMask enemyLayers;
    [Header("Damage")]
    public float damage = 40f;

    [Header("Audio")]
    public AudioClip mortarAttackSound; // Sonido del disparo de mortero

    readonly List<MortarBullet> pool = new();
    float fireCd;
    private AudioSource audioSource;

    void Awake()
    {
        if (!bulletPool) bulletPool = new GameObject($"{name}_MortarPool").transform;
        for (int i = 0; i < 10; i++) // Menos balas porque disparan lento
        {
            var go = Instantiate(mortarBullet, bulletPool);
            var p = go.GetComponent<MortarBullet>();
            if (p) { pool.Add(p); go.SetActive(false); }
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
            Transform target = AcquireTarget();
            if (target)
            {
                Fire(target.position); // <-- OJO: Pasamos la posición, no el Transform
                fireCd = 1f / fireRate;
            }
        }
    }

    void Fire(Vector3 targetPos)
    {
        // Reproducir sonido de disparo de mortero
        if (audioSource != null && mortarAttackSound != null)
        {
            audioSource.PlayOneShot(mortarAttackSound);
        }

        var projectile = pool.FirstOrDefault(x => !x.gameObject.activeInHierarchy);
        if (projectile)
        {
            projectile.transform.position = firePoint.position;
            projectile.damage = damage;
            projectile.enemyLayers = enemyLayers;
            // Lanzamos el proyectil hacia la posición donde ESTABA el enemigo
            projectile.Launch(targetPos);
        }
    }

    Transform AcquireTarget()
    {
        var hits = Physics.OverlapSphere(transform.position, range, enemyLayers);
        // Podrías mejorarlo para que busque al que está más lejos o con más vida
        if (hits.Length > 0) return hits[0].transform;
        return null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0.5f, 0, 0.3f); // Naranja para diferenciar
        Gizmos.DrawWireSphere(transform.position, range);
    }
}