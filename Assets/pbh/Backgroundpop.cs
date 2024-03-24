using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPop : MonoBehaviour
{
    public GameObject[] backgroundObjects; // Tableau contenant les objets de fond à faire apparaître
    public float minTimeBetweenSpawns = 1f; // Temps minimum entre deux apparitions d'objets
    public float maxTimeBetweenSpawns = 5f; // Temps maximum entre deux apparitions d'objets
    public Vector2 spawnAreaSize = new Vector2(10f, 10f); // Taille de la zone dans laquelle les objets apparaissent
    public Transform background; // Référence au GameObject contenant les objets devant le background

    private float nextSpawnTime;

    void Start()
    {
        // Initialisation du prochain moment d'apparition
        nextSpawnTime = Time.time + Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
    }

    void Update()
    {
        // Vérifie si le temps actuel est supérieur ou égal au prochain moment d'apparition
        if (Time.time >= nextSpawnTime)
        {
            SpawnBackgroundObject(); // Fait apparaître un objet de fond
            nextSpawnTime = Time.time + Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns); // Définit le prochain moment d'apparition
        }
    }

    void SpawnBackgroundObject()
    {
        // Sélectionne aléatoirement un objet de fond dans le tableau backgroundObjects
        GameObject selectedObject = backgroundObjects[Random.Range(0, backgroundObjects.Length)];

        // Calcule une position aléatoire dans la zone de spawn
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                                             Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
                                             0f);

        // Fait apparaître l'objet de fond à la position calculée
        GameObject spawnedObject = Instantiate(selectedObject, spawnPosition, Quaternion.identity);

    
        spawnedObject.transform.position = new Vector3(spawnedObject.transform.position.x, spawnedObject.transform.position.y, background.position.z - 1f);
    }
}