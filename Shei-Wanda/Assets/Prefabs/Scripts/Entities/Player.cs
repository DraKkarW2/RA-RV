using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : Entity
{
    // ********************************************** Propri�t�s avec encapsulation ********************************************** //
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

    private int _health;
    public int Health
    {
        get => _health;
        set => _health = Mathf.Clamp(value, 0, 100);
    }

    public bool Sprint { get; set; }
    public bool Exhausted { get; private set; }
    public int Money { get; set; }
    public List<Quest> QuestInfo { get; private set; } = new List<Quest>();

    // ********************************************** S�rialisation des contr�leurs ********************************************** //
    [SerializeField] private XRController leftController;   // R�f�rence au contr�leur gauche
    [SerializeField] private XRController rightController;  // R�f�rence au contr�leur droit


    // ********************************************** Initialisation ********************************************** //
    private void Awake()
    {
        if (leftController == null || rightController == null)
        {
            Debug.LogError("Left or Right controller is not assigned in the Inspector.");
        }
    }

    // ********************************************** Impl�mentation de la m�thode Move() ********************************************** //
    public override void Move()
    {
        Debug.Log($"Move method called.");

        // V�rifier si les contr�leurs sont assign�s
        if (leftController != null)
        {
            Vector2 inputAxis = Vector2.zero;

            // Essayez de r�cup�rer l'entr�e du joystick gauche
            if (leftController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis))
            {
                if (inputAxis != Vector2.zero)
                {
                    // Calculer la vitesse en fonction de si le joueur court ou marche
                    float moveSpeed = (Sprint && !Exhausted) ? Speed : Speed / 2;

                    // D�placement bas� sur l'input du joystick
                    Vector3 moveDirection = new Vector3(inputAxis.x, 0, inputAxis.y).normalized;
                    transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

                    // G�rer la Stamina en fonction du sprint
                    if (Sprint && !Exhausted)
                    {
                        Stamina -= 1;
                        Debug.Log($"{Name} is sprinting.");
                    }
                    else
                    {
                        Debug.Log($"{Name} is walking.");
                    }

                    // V�rifier si le joueur est �puis�
                    if (Stamina <= 0)
                    {
                        Exhausted = true;
                        Debug.Log("Player is exhausted.");
                    }
                }
            }
        }
    }

    // Mise � jour de l'entit� avec gestion sp�cifique du joueur
    public override void UpdateEntity()
    {
        base.UpdateEntity();
        UpdatePlayer();
    }

    // Mise � jour sp�cifique pour le joueur
    private void UpdatePlayer()
    {
        if (Exhausted)
        {
            Debug.Log("Player is exhausted.");
        }
    }

    // Interaction avec l'environnement
    public void Interact()
    {
        Debug.Log($"{Name} is interacting with the environment.");
    }

    // Prendre un objet
    public void TakeItem(Item item)
    {
        Debug.Log($"{Name} took {item.Name}");
    }
}
