using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : StateMachineBehaviour
{
   public GameObject enemyPrefab; // Le prefab de l'ennemi à spawn
    public int numberOfEnemies = 10; // Le nombre d'ennemis à spawn
    public float spawnRadius = 10f; // Le rayon autour du Transform pour la zone de spawn

    // OnStateEnter est appelé lorsqu'une transition commence et que la machine à états commence à évaluer cet état
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SpawnEnemies(animator.transform);
    }

    void SpawnEnemies(Transform spawnTransform)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Génère des positions aléatoires dans le rayon autour du Transform
            Vector3 randomPosition = GetRandomPositionInRadius(spawnTransform.position, spawnRadius);

            // Instancie l'ennemi à la position aléatoire
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPositionInRadius(Vector3 center, float radius)
    {
        // Génère des positions aléatoires dans le rayon autour du Transform
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        Vector3 randomPosition = new Vector3(randomCircle.x, 0f, randomCircle.y) + center;
        return randomPosition;
    }
}
