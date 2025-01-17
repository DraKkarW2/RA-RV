using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    public float AggressionLevel;
    public float VisionRange;
    public bool IsChasing;
    public float SpawnTime;

    [Header("Movement Settings")]
    public float rotationSpeed = 5f; // Vitesse de rotation vers la cible

    [SerializeField]
    private Player player; // Référence au joueur, sérialisée pour être assignée dans l'inspecteur

    private NavMeshAgent agent; // Référence au NavMeshAgent
    [SerializeField]
    private string entityName; // Champ affiché dans l'inspecteur

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

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Initialise le NavMeshAgent

        // Vérifie si le joueur est assigné
        if (player == null)
        {
            Debug.LogError($"{name} - Player reference is not assigned in the Inspector.");
        }
    }

    protected override void Update()
    {
        base.Update();

        // Vérifie si le joueur est valide
        if (player == null)
        {
            Debug.LogWarning($"{name} - Player reference is null. Skipping Update.");
            return;
        }

        // Détecte si le joueur est proche
        Debug.Log(NearTo(player));
        if (NearTo(player))
        {
            IsChasing = true;
        }
        else
        {
            IsChasing = false;
        }

        // Change de comportement en fonction de l'état
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

        // Déplace l'agent vers la position du joueur
        agent.SetDestination(player.Position);

        // Tourne doucement vers le joueur
        Vector3 direction = (player.Position - transform.position).normalized;
        if (direction != Vector3.zero) // Évite les rotations inutiles
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void Patrol()
    {
        if (!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            // Génère une position aléatoire proche de l'ennemi
            Vector3 randomDirection = Random.insideUnitSphere * 10; // Rayon de 10 unités pour la patrouille
            randomDirection += transform.position;

            // Trouve une position valide sur le NavMesh
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 10, NavMesh.AllAreas))
            {
                Vector3 patrolTarget = hit.position;
                agent.SetDestination(patrolTarget);
                Debug.Log($"{name} is patrolling to a new target: {patrolTarget}");
            }
        }
    }

    public bool NearTo(Player player)
    {
        float distance = Vector3.Distance(player.Position, Position);

        if (distance > VisionRange)
        {
            Debug.Log($"{name} doesn't see the player (out of range).");
            return false;
        }


        Vector3 directionToPlayer = (player.Position - transform.position).normalized;

        int layerMask = ~LayerMask.GetMask("enemy");

 
        Debug.DrawRay(transform.position + Vector3.up * 1.5f, directionToPlayer * VisionRange, Color.red, 0.1f);


        Ray ray = new Ray(transform.position + Vector3.up * 1.5f, directionToPlayer);
        if (Physics.Raycast(ray, out RaycastHit hit, VisionRange, layerMask, QueryTriggerInteraction.Ignore))
        {
       

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

    public void AttackPlayer(Player player, int damage)
    {
        if (IsChasing && Vector3.Distance(transform.position, player.Position) <= agent.stoppingDistance)
        {
            player.Health -= damage;
            Debug.Log($"{name} attacks the player for {damage} damage!");
        }
        else
        {
            Debug.Log($"{name} is not close enough to attack.");
        }
    }

    public override void Move()
    {
    }
}
