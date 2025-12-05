using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LightingTower : MonoBehaviour
{
    [Header("Refs")]
    public Transform firePoint;            // De dónde saldrán los rayos (ej. el cristal superior)
    [FormerlySerializedAs("lineRendererPrefab")] public GameObject lightingBullet; // Un prefab con un LineRenderer configurado

    [Header("Config General")]
    public float range = 18f;              // Rango de detección
    public LayerMask enemyLayers;
    public float fireRate = 0.8f;          // Disparos por segundo

    [Header("Config Rayo Encadenado")]
    public int maxChainJumps = 3;          // Cuántos enemigos puede golpear el rayo (incluido el primero)
    public float jumpRange = 8f;           // Qué tan lejos puede saltar el rayo de un enemigo a otro
    public float lightningDuration = 0.2f; // Cuánto dura el efecto visual del rayo
    public float damagePerJump = 30f;      // Daño por cada enemigo golpeado

    private List<LineRenderer> activeRays = new List<LineRenderer>(); // Para gestionar los efectos visuales
    private float fireCd;

    void Awake()
    {
        // Puedes crear un pool de LineRenderers si vas a tener muchos rayos simultáneos
        // Por simplicidad, los instanciamos y desactivamos/activamos.
        // Asegúrate de que el lineRendererPrefab tenga su GO desactivado por defecto.
    }

    void Update()
    {
        fireCd -= Time.deltaTime;
        if (fireCd <= 0f)
        {
            Transform target = AcquireTarget();
            if (target != null)
            {
                Attack(target);
                fireCd = 1f / fireRate;
            }
        }
    }

    void Attack(Transform primaryTarget)
    {
        // Escondemos todos los rayos antiguos antes de disparar nuevos (para evitar destellos)
        foreach (var lr in activeRays) { if (lr != null) lr.gameObject.SetActive(false); }

        List<Transform> hitTargets = new List<Transform>();
        Transform currentTarget = primaryTarget;
        Vector3 lastLightningPoint = firePoint.position;

        for (int i = 0; i < maxChainJumps; i++)
        {
            if (currentTarget == null || hitTargets.Contains(currentTarget)) break; // Ya golpeó a este o no hay objetivo

            hitTargets.Add(currentTarget);

            // 1. Dibuja el rayo desde el punto anterior al actual objetivo
            DrawLightningSegment(lastLightningPoint, currentTarget.position, lightningDuration);

            // 2. Aplica daño al objetivo actual
            var health = currentTarget.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damagePerJump);
                Debug.Log($"Rayo golpeó a {currentTarget.name} por {damagePerJump} de daño!");
            }

            // 3. Busca el siguiente objetivo para encadenar
            lastLightningPoint = currentTarget.position;
            currentTarget = FindNextChainTarget(currentTarget, hitTargets);
        }
    }

    Transform AcquireTarget()
    {
        // Encuentra el enemigo más cercano en el rango
        Collider[] hits = Physics.OverlapSphere(transform.position, range, enemyLayers);
        if (hits.Length == 0) return null;

        Transform closestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.gameObject.activeInHierarchy)
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestEnemy = hit.transform;
                }
            }
        }
        return closestEnemy;
    }

    Transform FindNextChainTarget(Transform currentEnemy, List<Transform> alreadyHit)
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(currentEnemy.position, jumpRange, enemyLayers);
        Transform nextEnemy = null;
        float minDistance = float.MaxValue;

        foreach (var hit in nearbyEnemies)
        {
            if (hit.gameObject.activeInHierarchy && !alreadyHit.Contains(hit.transform))
            {
                float dist = Vector3.Distance(currentEnemy.position, hit.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    nextEnemy = hit.transform;
                }
            }
        }
        return nextEnemy;
    }

    void DrawLightningSegment(Vector3 start, Vector3 end, float duration)
    {
        LineRenderer lr = GetPooledLineRenderer();
        lr.gameObject.SetActive(true);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        
        // Desactiva el rayo después de un tiempo para que no se quede dibujado
        StartCoroutine(DeactivateLightning(lr, duration));
    }

    // Coroutine para desactivar el rayo
    System.Collections.IEnumerator DeactivateLightning(LineRenderer lr, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (lr != null) lr.gameObject.SetActive(false);
    }

    // Simple pooling para LineRenderers
    LineRenderer GetPooledLineRenderer()
    {
        foreach (var lr in activeRays)
        {
            if (!lr.gameObject.activeInHierarchy) return lr;
        }
        GameObject newLrGo = Instantiate(lightingBullet);
        // Si no hay disponibles, crea uno nuevo
        LineRenderer newLr = newLrGo.GetComponentInChildren<LineRenderer>();
        activeRays.Add(newLr);
        return newLr;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
        if (firePoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(firePoint.position, 0.5f); // Punto de origen del rayo
        }
        // Dibuja el rango de salto del rayo si hay un objetivo
        if (fireCd <= 0 && FindObjectOfType<Enemy>() != null) // Solo para debugging si hay un enemigo
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * 2f, jumpRange);
        }
    }
}
