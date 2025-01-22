using UnityEngine;
using Unity.Netcode;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class PlayerData : NetworkBehaviour
{
    [Header("XR Origin & Action References")]
    [SerializeField] private XROrigin xrOrigin; // Référence à l'XR Origin
    public GameObject Camera; // Caméra associée au joueur
    [SerializeField] private InputActionAsset inputActions; // Asset des Input Actions assignées

    private ContinuousMoveProviderBase moveProvider;

    public override void OnNetworkSpawn()
    {
        Debug.Log($"[PlayerData][OnNetworkSpawn] Player {NetworkObjectId} spawned. IsOwner: {IsOwner}, IsHost: {IsHost}");
        if (IsOwner)
        {
            EnableOwnerComponents();
        }
        else
        {
            DisableNonOwnerComponents();
        }
    }

    private void EnableOwnerComponents()
    {
        Debug.Log($"[PlayerData][EnableOwnerComponents] Enabling components for owner {NetworkObjectId}...");

        // Activer la caméra
        if (Camera != null)
        {
            Camera.SetActive(true);
            Debug.Log($"[PlayerData][EnableOwnerComponents] Camera enabled for owner {NetworkObjectId}.");
        }
        else
        {
            Debug.LogWarning($"[PlayerData][EnableOwnerComponents] Camera reference is missing for player {NetworkObjectId}.");
        }

        // Activer le MoveProvider
        moveProvider = xrOrigin.GetComponent<ContinuousMoveProviderBase>();
        if (moveProvider != null)
        {
            moveProvider.enabled = true;
            Debug.Log($"[PlayerData][EnableOwnerComponents] MoveProvider enabled for owner {NetworkObjectId}.");
        }
        else
        {
            Debug.LogWarning($"[PlayerData][EnableOwnerComponents] MoveProvider is missing on the XR Origin for player {NetworkObjectId}.");
        }

        // Activer les Input Actions
        if (inputActions != null)
        {
            inputActions.Enable();
            Debug.Log($"[PlayerData][EnableOwnerComponents] Input Actions enabled for owner {NetworkObjectId}.");
        }
        else
        {
            Debug.LogWarning($"[PlayerData][EnableOwnerComponents] Input Actions are not assigned for player {NetworkObjectId}.");
        }

        DebugState($"EnableOwnerComponents for owner {NetworkObjectId}");
    }

    private void DisableNonOwnerComponents()
    {
        Debug.Log($"[PlayerData][DisableNonOwnerComponents] Disabling components for non-owner {NetworkObjectId}...");

        // Désactiver la caméra
        if (Camera != null)
        {
            Camera.SetActive(false);
            Debug.Log($"[PlayerData][DisableNonOwnerComponents] Camera disabled for non-owner {NetworkObjectId}.");
        }

        // Désactiver le MoveProvider
        moveProvider = xrOrigin.GetComponent<ContinuousMoveProviderBase>();
        if (moveProvider != null)
        {
            moveProvider.enabled = false;
            Debug.Log($"[PlayerData][DisableNonOwnerComponents] MoveProvider disabled for non-owner {NetworkObjectId}.");
        }

        // Désactiver les Input Actions
        if (inputActions != null)
        {
            inputActions.Disable();
            Debug.Log($"[PlayerData][DisableNonOwnerComponents] Input Actions disabled for non-owner {NetworkObjectId}.");
        }

        DebugState($"DisableNonOwnerComponents for non-owner {NetworkObjectId}");
    }

    private void DebugState(string context)
    {
        bool cameraActive = Camera != null && Camera.activeSelf;
        bool moveProviderEnabled = moveProvider != null && moveProvider.enabled;
        bool inputActionsEnabled = inputActions != null && inputActions.enabled;

        Debug.Log($"[PlayerData][{context}] Debugging state for Player {NetworkObjectId}: " +
                  $"IsOwner: {IsOwner}, IsHost: {IsHost}, " +
                  $"Camera active: {cameraActive}, MoveProvider enabled: {moveProviderEnabled}, InputActions enabled: {inputActionsEnabled}");
    }

    private void Update()
    {
        if (IsOwner)
        {
            bool cameraActive = Camera != null && Camera.activeSelf;
            bool moveProviderEnabled = moveProvider != null && moveProvider.enabled;
            bool inputActionsEnabled = inputActions != null && inputActions.enabled;

            Debug.Log($"[PlayerData][Update] Player {NetworkObjectId} - " +
                      $"Camera active: {cameraActive}, MoveProvider enabled: {moveProviderEnabled}, InputActions enabled: {inputActionsEnabled}");
        }
    }

    private void Start()
    {
        Debug.Log($"[PlayerData][Start] Player {NetworkObjectId} started. IsOwner: {IsOwner}, IsHost: {IsHost}");
    }
}
