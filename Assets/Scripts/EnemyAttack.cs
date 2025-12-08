using UnityEngine;

public class EnemyAttack : MonoBehaviour

{
    public Transform target;
    public float attackRange = 2f;
    public bool isAttacking = false;
    public Animator animator;
    public int damage = 10;

    [Header("Audio")]
    public AudioClip attackSound;
    [Range(0f, 1f)]
    public float attackVolume = 0.5f;
    private AudioSource audioSource;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Tower").transform;

        // Inicializar AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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
        // Reproducir sonido de ataque
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound, attackVolume);
        }

        if (target == null)
        {
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
