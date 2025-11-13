using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    [HideInInspector] public Transform destination;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
}