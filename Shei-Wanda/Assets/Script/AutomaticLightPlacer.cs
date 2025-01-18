using UnityEngine;

public class AutomaticLightPlacer : MonoBehaviour
{
    public GameObject lightPrefab; // Prefab de lumière à utiliser
    public float lightSpacing = 10f; // Distance entre les lumières
    public Vector3 areaSize = new Vector3(50, 10, 50); // Taille de la zone à couvrir
    public Vector3 offset = Vector3.zero; // Décalage par rapport au point d’origine
    public LayerMask groundLayer; // Couche utilisée pour détecter le sol

    void Start()
    {
        PlaceLights();
    }

    void PlaceLights()
    {
        // Calcul des limites de la zone
        Vector3 startPos = transform.position + offset - (areaSize / 2);
        Vector3 endPos = transform.position + offset + (areaSize / 2);

        for (float x = startPos.x; x <= endPos.x; x += lightSpacing)
        {
            for (float z = startPos.z; z <= endPos.z; z += lightSpacing)
            {
                // Trouver la position exacte sur le sol
                Vector3 lightPosition = new Vector3(x, startPos.y, z);

                if (Physics.Raycast(lightPosition + Vector3.up * 10, Vector3.down, out RaycastHit hit, 20f, groundLayer))
                {
                    // Placer la lumière au point d’impact du raycast
                    Instantiate(lightPrefab, hit.point, Quaternion.identity, transform);
                }
                else
                {
                    Debug.LogWarning($"Aucun sol détecté à la position {lightPosition}");
                }
            }
        }
    }
}
