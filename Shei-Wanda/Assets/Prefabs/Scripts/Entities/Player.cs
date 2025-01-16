using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : Entity
{
    // ********************************************** Propriétés avec encapsulation ********************************************** //
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

    // ********************************************** Sérialisation des contrôleurs ********************************************** //
    [SerializeField] private XRController leftController;   // Référence au contrôleur gauche
    [SerializeField] private XRController rightController;  // Référence au contrôleur droit


    // ********************************************** Initialisation ********************************************** //
    private void Awake()
    {
        if (leftController == null || rightController == null)
        {
            Debug.LogError("Left or Right controller is not assigned in the Inspector.");
        }
    }

    // ********************************************** Implémentation de la méthode Move() ********************************************** //
    public override void Move()
    {
        Debug.Log($"Move method called.");

        // Vérifier si les contrôleurs sont assignés
        if (leftController != null)
        {
            Vector2 inputAxis = Vector2.zero;

            // Essayez de récupérer l'entrée du joystick gauche
            if (leftController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis))
            {
                if (inputAxis != Vector2.zero)
                {
                    // Calculer la vitesse en fonction de si le joueur court ou marche
                    float moveSpeed = (Sprint && !Exhausted) ? Speed : Speed / 2;

                    // Déplacement basé sur l'input du joystick
                    Vector3 moveDirection = new Vector3(inputAxis.x, 0, inputAxis.y).normalized;
                    transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

                    // Gérer la Stamina en fonction du sprint
                    if (Sprint && !Exhausted)
                    {
                        Stamina -= 1;
                        Debug.Log($"{Name} is sprinting.");
                    }
                    else
                    {
                        Debug.Log($"{Name} is walking.");
                    }

                    // Vérifier si le joueur est épuisé
                    if (Stamina <= 0)
                    {
                        Exhausted = true;
                        Debug.Log("Player is exhausted.");
                    }
                }
            }
        }
    }

    // Mise à jour de l'entité avec gestion spécifique du joueur
    public override void UpdateEntity()
    {
        base.UpdateEntity();
        UpdatePlayer();
    }

    // Mise à jour spécifique pour le joueur
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
