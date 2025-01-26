using UnityEngine;
using UnityEngine.AI;
using System.Collections;

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
    public AudioSource chaseAudioSource;
    public AudioSource patrolAudioSource;
    public AudioSource hitAudioSource;

    private bool isPausedAfterHit = false;  // Empêche les changements d'état après une attaque
    private bool isPlayingPatrolAudio = false;  // Vérifie si la musique de patrouille est déjà en cours
    private bool isPlayingChaseAudio = false;   // Vérifie si la musique de chasse est déjà en cours
    private float attackCooldown = 1.5f;       // Temps minimum entre deux attaques
    private float lastAttackTime;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            Debug.LogError($"{name} - Player reference is not assigned in the Inspector.");
        }

        // Vérification des AudioSources
        if (patrolAudioSource == null)
        {
            Debug.LogWarning($"{name} - Patrol audio source is not assigned.");
        }

        if (chaseAudioSource == null)
        {
            Debug.LogWarning($"{name} - Chase audio source is not assigned.");
        }

        if (hitAudioSource == null)
        {
            Debug.LogWarning($"{name} - Hit audio source is not assigned.");
        }
    }

    protected override void Update()
    {
        base.Update();

        if (player == null) return;

        // Si le joueur est mort, patrouille
        if (player.Health <= 0)
        {
            IsChasing = false;
            Patrol();
            StopAudio();
            return;
        }

        // Si une pause après une attaque est en cours, empêcher les changements d'état
        if (isPausedAfterHit) return;

        // Logique de détection du joueur
        if (NearTo(player))
        {
            if (!IsChasing)
            {
                IsChasing = true;
                PlayChaseAudio(); // Joue la musique de chasse seulement si nécessaire
                Debug.Log($"{name}: Started chasing the player.");
            }
        }
        else
        {
            if (IsChasing)
            {
                IsChasing = false;
                Patrol(); // Passe en mode patrouille seulement si nécessaire
                Debug.Log($"{name}: Lost the player. Switching to patrol.");
            }
        }

        // Actions selon l'état
        if (IsChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        // Met à jour les volumes des sons en fonction de la distance
        UpdateAudioVolumes();
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        agent.SetDestination(player.Position);

        if (Vector3.Distance(transform.position, player.Position) <= 8)
        {
            AttackPlayer(player, 10);
        }

        Vector3 direction = (player.Position - transform.position).normalized;
        if (direction != Vector3.zero)
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
            Vector3 patrolTarget = Vector3.zero;
            bool validPath = false;

            for (int i = 0; i < 10; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * 10;
                randomDirection += transform.position;

                if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 10, NavMesh.AllAreas))
                {
                    patrolTarget = hit.position;
                    NavMeshPath path = new NavMeshPath();
                    if (agent.CalculatePath(patrolTarget, path) && path.status == NavMeshPathStatus.PathComplete)
                    {
                        validPath = true;
                        break;
                    }
                }
            }

            if (validPath)
            {
                agent.SetDestination(patrolTarget);
            }
            else
            {
                Debug.LogWarning($"{name}: No valid patrol position found after multiple attempts.");
            }
        }

        PlayPatrolAudio(); // Joue la musique de patrouille seulement si nécessaire
    }

    public bool NearTo(Player player)
    {
        float distance = Vector3.Distance(player.Position, Position);

        if (distance > VisionRange)
        {
            return false;
        }

        Vector3 directionToPlayer = (player.Position - transform.position).normalized;

        int layerMask = ~LayerMask.GetMask("enemy", "HUD");

        Ray ray = new Ray(transform.position + Vector3.up * 1.5f, directionToPlayer);
        if (Physics.Raycast(ray, out RaycastHit hit, VisionRange, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    public void AttackPlayer(Player player, int damage)
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            player.TakeDamage(damage);
            lastAttackTime = Time.time;

            if (hitAudioSource != null)
            {
                hitAudioSource.Play();
                Debug.Log($"{name}: Playing hit audio.");
            }

            StartCoroutine(PauseAfterHit());
        }
    }

    private IEnumerator PauseAfterHit()
    {
        isPausedAfterHit = true;

        yield return new WaitForSeconds(1.0f);

        isPausedAfterHit = false;
    }

    private void PlayChaseAudio()
    {
        if (isPlayingChaseAudio) return;

        Debug.Log($"{name}: Starting chase audio.");
        isPlayingChaseAudio = true;
        isPlayingPatrolAudio = false;

        if (patrolAudioSource != null && patrolAudioSource.isPlaying)
        {
            patrolAudioSource.Stop();
            Debug.Log($"{name}: Stopping patrol audio.");
        }

        if (chaseAudioSource != null && !chaseAudioSource.isPlaying)
        {
            chaseAudioSource.Play();
        }
    }

    private void PlayPatrolAudio()
    {
        if (isPlayingPatrolAudio) return;

        Debug.Log($"{name}: Starting patrol audio.");
        isPlayingPatrolAudio = true;
        isPlayingChaseAudio = false;

        if (chaseAudioSource != null && chaseAudioSource.isPlaying)
        {
            chaseAudioSource.Stop();
            Debug.Log($"{name}: Stopping chase audio.");
        }

        if (patrolAudioSource != null && !patrolAudioSource.isPlaying)
        {
            patrolAudioSource.Play();
        }
    }

    private void StopAudio()
    {
        Debug.Log($"{name}: Stopping all audio.");
        isPlayingChaseAudio = false;
        isPlayingPatrolAudio = false;

        if (chaseAudioSource != null && chaseAudioSource.isPlaying)
        {
            chaseAudioSource.Stop();
        }

        if (patrolAudioSource != null && patrolAudioSource.isPlaying)
        {
            patrolAudioSource.Stop();
        }
    }

    private void UpdateAudioVolumes()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.Position);

        if (patrolAudioSource != null)
        {
            patrolAudioSource.volume = Mathf.Clamp01(1 - (distanceToPlayer / patrolAudioSource.maxDistance));
        }

        if (chaseAudioSource != null)
        {
            chaseAudioSource.volume = Mathf.Clamp01(1 - (distanceToPlayer / chaseAudioSource.maxDistance));
        }
    }

    public override void Move()
    {

    }
}
