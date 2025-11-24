using UnityEngine;

public class Enemy : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform destination;
    public Animator animator;
    private int speedHash = Animator.StringToHash("MoveSpeed");

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Set the destination to the TowerTag1
        if (destination != null)
        {
            agent.SetDestination(destination.position);
        }
        else
        {
            Debug.LogError("Destination is not assigned! Please assign the TowerTag1 GameObject to the destination field in the Inspector.");
        }
    }

    private void Update()
    {
        // Continuously move towards the destination
        if (destination != null)
        {
            agent.SetDestination(destination.position);
        }
        animator.SetFloat(speedHash, agent.velocity.magnitude / agent.speed);
    }
}