using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab du joueur
    private GameObject[] spawnPoints; // Liste des points de spawn

    void Start()
    {
        // Trouver tous les points de spawn dans la scène
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Aucun point de spawn trouvé ! Assurez-vous que les GameObjects SpawnPoint ont le bon tag.");
            return;
        }

        // Appeler la méthode pour spawner le joueur
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (playerPrefab != null && spawnPoints.Length > 0)
        {
            // Choisir un point de spawn aléatoire
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex].transform;

            Debug.Log($"Spawning player at position: {spawnPoint.position}");

            // Instancier le joueur à cet emplacement
            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Prefab de joueur ou points de spawn non définis !");
        }
    }
}
