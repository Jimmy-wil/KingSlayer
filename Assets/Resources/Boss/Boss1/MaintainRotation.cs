using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainPosition : MonoBehaviour
{
// Angle que nous voulons maintenir
    private Quaternion targetRotation;

    // Référence à l'Animator
    private Animator animator;

    // Valeur pour décaler la position X de Weapon
    public float xOffset = 1.0f;

    // Position initiale de Weapon
    private Vector3 initialPosition;

    void Start()
    {
        // Récupérer l'Animator attaché à ce GameObject
        animator = GetComponent<Animator>();

        // Définir la rotation cible à 0 degrés sur les axes X et Z
        targetRotation = Quaternion.Euler(targetRotation.x, 0, targetRotation.z);

        // Sauvegarder la position initiale
        initialPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        // Vérifier si le paramètre "Enraged" de l'Animator est vrai
        // ET que le trigger "Attack" n'est pas actif
        if (animator != null && animator.GetBool("Enraged") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            targetRotation = Quaternion.Euler(targetRotation.x, 0, 360);
            // Appliquer la rotation cible indépendamment de la rotation du parent
            transform.rotation = targetRotation;

            // Ajouter la valeur xOffset à la position initiale sur l'axe X
            Vector3 newPosition = initialPosition;
            newPosition.x += xOffset;
            transform.localPosition = newPosition;
        }
    }
}
