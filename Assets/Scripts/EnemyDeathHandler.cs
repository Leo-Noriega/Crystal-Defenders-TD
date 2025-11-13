using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    public void HandleDeath()
    {
        gameObject.SetActive(false);
        // sumar monedas o sonidos de muerte aqui
    }
}
