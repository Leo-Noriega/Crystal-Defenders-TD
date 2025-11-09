using UnityEngine;

public class Enemy : MonoBehaviour
{
	
	
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform destination;
    private Collider[] detectedObjects = new Collider[10];
    public float detectionRadius = 107f;
    public Transform player;
	
    // Categorias para tener acceso ordenado a nuestras variables
    
    [Header("Patrolling State Variables")]
    public Transform[] waypoints;
    
    public bool isPlayerSighted;
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
	
	private void FixedUpdate()
	{
		// crea una esphera que servira como rango de deteccion de coliciones
		var numberOfColliders = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, detectedObjects);
		isPlayerSighted = false; 
		
		for (int i = 0; i < numberOfColliders; i++)
		{
			var col = detectedObjects[i];
			if (!col.CompareTag("towerTag1")) continue; // si no es el jugador continua (Filtro1)
			player = col.transform;
			var vectorPlayer = player.position - transform.position;
			// el dot es la evalucion de que tan aliniados estan esos vectores
			var dot = Vector3.Dot(vectorPlayer, transform.forward); 
			if (dot < 0) continue; // si esta hacia atras continua (Filtro 2)
			// deteccion de obstaculos con rayo
			Physics.Raycast(transform.position, vectorPlayer, out var hit);
			// si el punto con el que choca el rayo es el jugador entonces se podria decir que el agente esta viendo al jugador
			if (hit.collider.transform != player) continue; // (Filtro 3)
			
			isPlayerSighted = true;
			
			// agent.destination = player.position;
		}
	}

	private void OnDrawGizmos()
	{
		// Sirve para vizualizar el rango de deteccion
		//Gizmos.color = new Color(0.6f,0.05f,1f, 0.5f);
		//Gizmos.DrawSphere(transform.position, detectionRadius);
		
	}

    

}
