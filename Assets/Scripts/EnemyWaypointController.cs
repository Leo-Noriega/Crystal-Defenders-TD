using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyWaypointController : MonoBehaviour
{
    public Transform[] waypoints;      // Tus waypoints + torre final
    private int currentIndex = 0;
    private Enemy enemy;
    private NavMeshAgent agent;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        agent = enemy.agent;

        if (waypoints.Length > 0)
        {
            enemy.destination = waypoints[currentIndex];
            agent.SetDestination(enemy.destination.position);
        }
        else
        {
            Debug.LogError("Waypoints no asignados en EnemyWaypointController");
        }
    }

    void Update()
    {
        if (enemy.destination == null || waypoints.Length == 0) return;

        // Usa remainingDistance del NavMeshAgent para detectar llegada
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentIndex++;

            if (currentIndex < waypoints.Length)
            {
                // Siguiente waypoint
                enemy.destination = waypoints[currentIndex];
                agent.SetDestination(enemy.destination.position);
            }
            else
            {
                // Llegó al final del recorrido
                Destroy(gameObject); // o daño a la torre
            }
        }
    }
}