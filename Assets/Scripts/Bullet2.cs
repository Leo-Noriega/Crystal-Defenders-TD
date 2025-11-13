using UnityEngine;

public class Bullet2 : MonoBehaviour
{
    public float damage = 10f;

    public float lifeSeconds = 5f;
    private float life;

    private void OnEnable()
    {
        life = lifeSeconds;
    }

    private void Update()
    {
        life -= Time.deltaTime;
        if (life <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TowerHealth towerHealth = other.GetComponent<TowerHealth>();
        if (towerHealth != null)
        {
            // Aplicar daño
            towerHealth.TakeDamage(damage);

            // Destruir la bala al impactar
            gameObject.SetActive(false);
        }
    }
}