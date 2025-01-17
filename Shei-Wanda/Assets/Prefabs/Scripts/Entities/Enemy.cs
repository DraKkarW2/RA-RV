using UnityEngine;

public class Enemy : Entity
{
    public float AggressionLevel; 
    public float VisionRange; 
    public bool IsChasing;
    public float SpawnTime; 

    public void Transform()
    {
        Debug.Log($"{name} is transforming!");
       
    }

    public void Patrol()
    {
        Debug.Log($"{name} is patrolling.");
    }

    public bool NearTo(Player player)
    {
        float distance = Vector3.Distance(player.Position, Position);
        bool isNear = distance <= VisionRange;

        Debug.Log(isNear
            ? $"{name} sees the player and is within range."
            : $"{name} doesn't see the player.");

        return isNear;
    }

    public void AttackPlayer(Player player, int damage)
    {
        if (IsChasing)
        {
            player.Health -= damage;
            Debug.Log($"{name} attacks the player for {damage} damage!");
        }
        else
        {
            Debug.Log($"{name} is not chasing the player and cannot attack.");
        }
    }
    public override void Move()
    {
        throw new System.NotImplementedException();
    }
}
