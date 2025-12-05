// csharp
        // File: `Assets/Scripts/DamageProjectile.cs`
        using System.Collections;
        using UnityEngine;
        
        public class DamageProjectile : MonoBehaviour
        {
            [SerializeField] private float damage = 25f;
            public void SetDamage(float newDamage)
            {
                damage = newDamage;
            }
            [SerializeField] private float destroyDelay = 0.05f;
            [SerializeField] private LayerMask targetLayers;
            
            private bool hasHit = false;
            private Collider col;
            private Rigidbody rb;
        
            private void Awake()
            {
                col = GetComponent<Collider>();
                rb = GetComponent<Rigidbody>();
            }
        
            private void OnEnable()
            {
                hasHit = false;
                if (col != null) col.enabled = true;
            }
        
            private void HandleHit(GameObject otherObj)
            {
                if (hasHit) return;
        
                // 1) Filtrar por layer
                if ((targetLayers.value & (1 << otherObj.layer)) == 0)
                    return; // No es un objetivo válido

                // 2) Buscar Health
                var health = otherObj.GetComponent<EnemyHealth>();
                if (health == null) return;

                hasHit = true;
                health.TakeDamage(damage);

                // 3) Detener movimiento y desactivar collider
                if (rb != null) rb.linearVelocity = Vector3.zero;
                if (col != null) col.enabled = false;

                if (destroyDelay > 0f)
                {
                    StartCoroutine(DisableAfterDelay());
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        
            private void OnTriggerEnter(Collider other)
            {
                HandleHit(other.gameObject);
            }
        
            private void OnCollisionEnter(Collision collision)
            {
                HandleHit(collision.gameObject);
            }
        
            private IEnumerator DisableAfterDelay()
            {
                yield return new WaitForSeconds(destroyDelay);
                gameObject.SetActive(false);
            }
        }