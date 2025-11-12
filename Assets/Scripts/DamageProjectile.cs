// csharp
        // File: `Assets/Scripts/DamageProjectile.cs`
        using System.Collections;
        using UnityEngine;
        
        public class DamageProjectile : MonoBehaviour
        {
            [SerializeField] private float damage = 25f;
            [SerializeField] private float destroyDelay = 0.05f;
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
        
                if (otherObj == null) return;
                var health = otherObj.GetComponent<Health>();
                if (health != null)
                {
                    hasHit = true;
                    health.TakeDamage(damage);
        
                    // detener movimiento si hay Rigidbody
                    if (rb != null) rb.linearVelocity = Vector3.zero;
        
                    // deshabilitar collider para evitar múltiples colisiones
                    if (col != null) col.enabled = false;
        
                    StartCoroutine(DisableAfterDelay());
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