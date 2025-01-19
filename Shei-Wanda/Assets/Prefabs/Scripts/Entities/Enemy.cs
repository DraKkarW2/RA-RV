using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    public float AggressionLevel;
    public float VisionRange;
    public bool IsChasing;
    public float SpawnTime;

    [Header("Movement Settings")]
    public float rotationSpeed = 5f; 

    [SerializeField]
    private Player player; 

    private NavMeshAgent agent; 
    [SerializeField]
    private string entityName; 

    public new string Name
    {
        get => entityName;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                entityName = value;
            else
                Debug.LogWarning("Name cannot be null or empty.");
        }
    }
 
    [Header("Audio Sources")]
    public AudioSource movementAudioSource; 
    public AudioSource chaseAudioSource;
    [Header("Hit Audio")]
    public AudioSource hitAudioSource;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            Debug.LogError($"{name} - Player reference is not assigned in the Inspector.");
        }
    }

    protected override void Update()
    {
        base.Update();

        if (player == null)
        {
            Debug.LogWarning($"{name} - Player reference is null. Skipping Update.");
            return;
        }
        if (player.Health <= 0)
        {
            IsChasing = false;
            Patrol();
            StopAudio();
            return;
        }
        if (NearTo(player))
        {
            if (!IsChasing) PlayChaseAudio();
            IsChasing = true;
        }
        else
        {
            if (IsChasing) PlayMovementAudio();
            IsChasing = false;
        }

        if (IsChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        agent.SetDestination(player.Position);
        if (Vector3.Distance(transform.position, player.Position) <= agent.stoppingDistance)
        {
            AttackPlayer(player, 10); 
        }
        Vector3 direction = (player.Position - transform.position).normalized;
        if (direction != Vector3.zero) // Évite les rotations inutiles
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void Patrol()
    {


        if (agent.isStopped)
        {
            agent.isStopped = false;
        }

        if (agent.hasPath && agent.remainingDistance > 0.5f && agent.velocity.magnitude < 0.1f)
        {
            agent.ResetPath();
        }

        if (!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 10; 
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 10, NavMesh.AllAreas))
            {
                Vector3 patrolTarget = hit.position;

                agent.SetDestination(patrolTarget);
            }
        }
    }


    public bool NearTo(Player player)
    {
        float distance = Vector3.Distance(player.Position, Position);
        if (distance > VisionRange)
        {
          //Debug.Log($"{name} doesn't see the player (out of range).");
            return false;
        }


        Vector3 directionToPlayer = (player.Position - transform.position).normalized;

        int layerMask = ~LayerMask.GetMask("enemy", "HUD");


        Debug.DrawRay(transform.position + Vector3.up * 1.5f, directionToPlayer * VisionRange, Color.red, 0.1f);


        Ray ray = new Ray(transform.position + Vector3.up * 1.5f, directionToPlayer);
        if (Physics.Raycast(ray, out RaycastHit hit, VisionRange, layerMask, QueryTriggerInteraction.Ignore))
        {

            Debug.Log(hit.collider.CompareTag("Player"));
            // Si le Raycast touche le player
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        Debug.Log($"{name} Raycast did not hit anything.");
        return false;
    }
    private float attackCooldown = 1.5f;
    private float lastAttackTime;
    public void AttackPlayer(Player player, int damage)
    {
        //Debug.Log($"{name} is attaking");
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            player.TakeDamage(damage);
            lastAttackTime = Time.time;
        }
        if (hitAudioSource != null)
        {
            hitAudioSource.Play();
        }
    }

    public override void Move()
    {
    }
    // Gère les sons
    private void PlayMovementAudio()
    {
        if (movementAudioSource != null && !movementAudioSource.isPlaying)
        {
            chaseAudioSource.Stop();
            movementAudioSource.Play();
        }
    }

    private void PlayChaseAudio()
    {
        if (chaseAudioSource != null && !chaseAudioSource.isPlaying)
        {
            movementAudioSource.Stop();
            chaseAudioSource.Play();
        }
    }

    private void StopAudio()
    {
        if (movementAudioSource != null) movementAudioSource.Stop();
        if (chaseAudioSource != null) chaseAudioSource.Stop();
    }
}
