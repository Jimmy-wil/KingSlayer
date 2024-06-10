using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBehavior : MonoBehaviour
{
    public Transform anchorPoint; // Référence au point d'ancrage
    public float maxDistance = 10.0f; // Distance maximale avant de revenir au point d'ancrage
    public float returnSpeed = 3.5f; // Vitesse à laquelle l'ennemi revient au point d'ancrage

    void Update()
    {
        // Vérifie la distance entre l'ennemi et le point d'ancrage
        float distanceToAnchor = Vector2.Distance(transform.position, anchorPoint.position);

        if (distanceToAnchor > maxDistance)
        {
            // Calcule la direction du point d'ancrage
            Vector2 direction = (anchorPoint.position - transform.position).normalized;

            // Déplace l'ennemi vers le point d'ancrage
            transform.position = Vector2.MoveTowards(transform.position, anchorPoint.position, returnSpeed * Time.deltaTime);
        }
    }
}