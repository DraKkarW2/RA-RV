using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using Unity.Netcode;

public class Player : Entity
{
    // =============== Propriétés (Encapsulation) ===============
    private int _stamina;
    public int Stamina
    {
        get => _stamina;
        set
        {
            _stamina = Mathf.Max(0, value);
            Exhausted = _stamina <= 0;
        }
    }

    private int _sanity;
    public int Sanity
    {
        get => _sanity;
        set => _sanity = Mathf.Max(0, value);
    }

    [SerializeField] private int _health = 100; 
    public int Health
    {
        get => _health;
        set => _health = Mathf.Clamp(value, 0, 100);
    }

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
    public bool Sprint { get; set; }
    public bool Exhausted { get; private set; }
    public int Money { get; set; }


    [Header("XR Origin & Action References")]
    [SerializeField] private XROrigin xrOrigin;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference sprintAction;

    [Header("Vitesse de Déplacement")]
    [SerializeField] private float normalSpeed = 7f;
    [SerializeField] private float sprintSpeed = 12f;

    private ActionBasedController leftController;
    private ActionBasedController rightController;
    private ContinuousMoveProviderBase moveProvider;

    [Header("Damage Effect")]
    [SerializeField] private DamageEffect damageEffect;

    // ==================== Propriété redéfinie ====================
    public override Vector3 Position
    {
        get => xrOrigin ? xrOrigin.transform.position : transform.position;
        set
        {
            if (xrOrigin)
            {
                xrOrigin.transform.position = value;
            }
            else
            {
                transform.position = value;
            }
        }
    }

    private void Awake()
    {
        if (xrOrigin == null)
        {
            xrOrigin = GetComponentInChildren<XROrigin>(true);
        }

        if (xrOrigin == null)
        {
            Debug.LogError("XROrigin introuvable.");
        }
        else
        {
            leftController = xrOrigin.transform.Find("Camera Offset/Left Controller")
                ?.GetComponent<ActionBasedController>();
            rightController = xrOrigin.transform.Find("Camera Offset/Right Controller")
                ?.GetComponent<ActionBasedController>();

            moveProvider = xrOrigin.GetComponentInChildren<ContinuousMoveProviderBase>();
        }

        if (moveAction == null)
            Debug.LogError("moveAction n'est pas assigné dans l'Inspector.");
        if (sprintAction == null)
            Debug.LogError("sprintAction n'est pas assigné dans l'Inspector.");
        if (moveProvider == null)
            Debug.LogError("ContinuousMoveProvider introuvable sous le XR Origin.");
    }

    private void OnEnable()
    {
        if (sprintAction != null)
        {
            sprintAction.action.Enable();
            sprintAction.action.performed += StartSprint;
            sprintAction.action.canceled += StopSprint;
        }

        if (moveAction != null)
        {
            moveAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (sprintAction != null)
        {
            sprintAction.action.performed -= StartSprint;
            sprintAction.action.canceled -= StopSprint;
            sprintAction.action.Disable();
        }

        if (moveAction != null)
        {
            moveAction.action.Disable();
        }
    }

    private void StartSprint(InputAction.CallbackContext context)
    {
        if (!Exhausted && moveProvider != null)
        {
            Sprint = true;
            moveProvider.moveSpeed = sprintSpeed;
            Debug.Log("Sprint activé (Action-based).");
        }
    }

    private void StopSprint(InputAction.CallbackContext context)
    {
        if (moveProvider != null)
        {
            Sprint = false;
            moveProvider.moveSpeed = normalSpeed;
            Debug.Log("Sprint désactivé (Action-based).");
        }
    }

    public override void Move()
    {
        if (moveAction != null && moveAction.action != null)
        {
            Vector2 inputAxis = moveAction.action.ReadValue<Vector2>();
            //Debug.Log($"Action-based input Axis: {inputAxis}");
        }
    }
    public void TakeDamage(int damage)
    {
        Health -= damage;
        //Debug.Log($"Player took {damage} damage! Health: {Health}");

        // Déclenche l'effet visuel de dégâts
        if (damageEffect != null)
        {
            damageEffect.TriggerDamageEffect();
        }
        else
        {
            Debug.LogWarning("DamageEffect is not assigned in the Player script.");
        }

        // Vérifie si le joueur est mort
        if (Health <= 0)
        {
            Die();
        }
    }



    public override void Die()
    {
        Debug.Log("Player is dead.");
    }

    public override void UpdateEntity()
    {
        base.UpdateEntity();

        if (Sprint && !Exhausted)
        {
            Stamina--;
            Debug.Log($"Stamina: {Stamina}");

            if (Stamina <= 0)
            {
                Exhausted = true;
                Sprint = false;
                if (moveProvider != null)
                {
                    moveProvider.moveSpeed = normalSpeed;
                }
                Debug.Log("Player est épuisé (Action-based).");
            }
        }
        else if (Stamina < 100)
        {
            Stamina++;
        }

    }
}
