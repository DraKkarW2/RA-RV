using UnityEngine;

public class AutomaticLightPlacer : MonoBehaviour

{
    public GameObject lightPrefab;  // Prefab de la lumière
    public GameObject ground;  // Objet représentant le sol
    public int numberOfLights = 10;  // Nombre de lumières à placer
    public float lightHeight = 3f;  // Hauteur des lumières au-dessus du sol

    void Start()
    {
        if (ground == null)
        {
            Debug.LogError("Ground object is not assigned in the inspector!");
            return;
        }
        PlaceRandomLights();
    }

    void PlaceRandomLights()
    {
        Renderer groundRenderer = ground.GetComponent<Renderer>();
        if (groundRenderer == null)
        {
            Debug.LogError("Ground object does not have a Renderer component!");
            return;
        }

        Vector3 groundSize = groundRenderer.bounds.size;
        Vector3 groundPosition = ground.transform.position;

        for (int i = 0; i < numberOfLights; i++)
        {
            float randomX = Random.Range(groundPosition.x - groundSize.x / 2, groundPosition.x + groundSize.x / 2);
            float randomZ = Random.Range(groundPosition.z - groundSize.z / 2, groundPosition.z + groundSize.z / 2);
            Vector3 lightPosition = new Vector3(randomX, groundPosition.y + lightHeight, randomZ);

            Instantiate(lightPrefab, lightPosition, Quaternion.identity);
            Debug.Log($"Light placed at {lightPosition}");
        }
    }
}
