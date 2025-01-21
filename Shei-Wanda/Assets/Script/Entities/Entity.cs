using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    // ********************************************** Propriétés avec encapsulation ********************************************** //
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                _name = value;
            else
                Debug.LogWarning("Name cannot be null or empty.");
        }
    }

    private Vector3 _position;
    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
            transform.position = _position;
        }
    }

    private int _speed;
    public int Speed
    {
        get => _speed;
        set => _speed = Mathf.Max(0, value);
    }

    private Vector3 _spawnPoint;
    public Vector3 SpawnPoint
    {
        get => _spawnPoint;
        set => _spawnPoint = value;
    }

    private Sprite _entityModel;
    public Sprite EntityModel
    {
        get => _entityModel;
        set
        {
            _entityModel = value;
            Debug.Log($"{Name} changed its model.");
        }
    }

    // ********************************************** Fonctions de la classe ********************************************** //

    public abstract void Move();

    public virtual void Die()
    {
        Debug.Log($"{Name} is dead.");
        Destroy(gameObject);
    }

    public virtual void UpdateEntity()
    {
        
    }

    protected virtual void Update()
    {
        UpdateEntity();
        Move();
    }

    public void Teleport(Vector3 position)
    {
        transform.position = position;
        Debug.Log($"{Name} teleported to {position}");
    }

    public void ChangeModel(Sprite model)
    {
        EntityModel = model;
        Debug.Log($"{Name} changed its model.");
    }
}
