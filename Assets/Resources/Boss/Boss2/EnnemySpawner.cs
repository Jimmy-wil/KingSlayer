using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawner : MonoBehaviour
{
   public GameObject enemyPrefab; // Le prefab de l'ennemi à spawn
    public int numberOfEnemies = 10; // Le nombre d'ennemis à spawn
    public BoxCollider spawnZone; // Le BoxCollider définissant la zone de spawn

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        if (spawnZone == null)
        {
            Debug.LogError("Spawn zone is not assigned!");
            return;
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Génère des positions aléatoires dans la zone définie par le BoxCollider
            Vector3 randomPosition = GetRandomPositionInZone(spawnZone.bounds);

            // Instancie l'ennemi à la position aléatoire
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPositionInZone(Bounds bounds)
    {
        // Génère des positions aléatoires à l'intérieur des limites du BoxCollider
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, randomY, randomZ);
    }
}
