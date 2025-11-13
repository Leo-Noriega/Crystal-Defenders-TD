using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyWaypointController : MonoBehaviour
{
    private Transform[] waypoints;
    private int currentIndex = 0;

    private Enemy enemy;
    private NavMeshAgent agent;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        agent = enemy.agent;

        LoadWaypoints();
    }

    void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("No hay Waypoints en la escena.");
            return;
        }

        currentIndex = 0;
        MoveToWaypoint(currentIndex);
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        // Usamos un margen pequeño en lugar de stoppingDistance para evitar que se quede atorado
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            currentIndex++;
            if (currentIndex < waypoints.Length)
            {
                MoveToWaypoint(currentIndex);
            }
            else
            {
                // Llegó al final → se detiene y desaparece
                agent.isStopped = true;
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }

    private void MoveToWaypoint(int index)
    {
        if (waypoints == null || index >= waypoints.Length) return;

        agent.isStopped = false;
        agent.SetDestination(waypoints[index].position);
    }

    private void LoadWaypoints()
    {
        GameObject parent = GameObject.Find("Waypoints");
        if (parent == null)
        {
            Debug.LogError("No se encontró el objeto Waypoints.");
            return;
        }

        int count = parent.transform.childCount;
        waypoints = new Transform[count];

        for (int i = 0; i < count; i++)
        {
            waypoints[i] = parent.transform.GetChild(i);
        }
    }
}