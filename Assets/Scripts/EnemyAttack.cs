using UnityEngine;

public class EnemyAttack : MonoBehaviour

{
    public Transform target;
    public float attackRange = 2f;
    public bool isAttacking = false;
    public Animator animator;
    public int damage = 10;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Tower").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (target != null)
        {
            // Calculate the distance between the enemy and the target
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // Check if the enemy is within shooting range and it's time to shoot
            if (distanceToTarget <= attackRange && !isAttacking)
            {
                isAttacking = true;
                PlayAnimation();
            }
        }
        
        
    }

    public void EndAttackAnimation()
    {
        animator.CrossFadeInFixedTime("Movement", 0.2f);
        isAttacking = false;
    }

    private void PlayAnimation()
    {
        animator.CrossFadeInFixedTime("Attack", 0.2f);
        
    }

    private void Attack()
    {
        
        //Debug.Log("[EnemyShooting] Attack() fue llamado");

        if (target == null)
        {
            //Debug.LogWarning("[EnemyShooting] target es NULL, no puedo atacar.");
            return;
        }

        // Intentar encontrar TowerHealth en el target
        TowerHealth th = target.GetComponent<TowerHealth>();

        if (th == null)
        {
            //Debug.LogWarning($"[EnemyShooting] El target {target.name} NO tiene TowerHealth. ¿Seguro que es la torre correcta?");
            return;
        }

        //Debug.Log($"[EnemyShooting] Vida ANTES del daño: {th.currentHealth}");
        th.TakeDamage(damage);
        //Debug.Log($"[EnemyShooting] Vida DESPUÉS del daño: {th.currentHealth}");
        
    }
}
