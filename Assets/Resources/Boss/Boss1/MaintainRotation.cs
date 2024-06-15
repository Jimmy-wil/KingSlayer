using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainPosition : MonoBehaviour
{
    // Angle que nous voulons maintenir
    private Quaternion targetRotation;

    // Référence à l'Animator
    private Animator animator;

    void Start()
    {
        // Récupérer l'Animator attaché à ce GameObject
        animator = GetComponent<Animator>();

        // Définir la rotation cible à 90 degrés sur les axes X et Z
        targetRotation = Quaternion.Euler(0, 0, 0);
    }

    void LateUpdate()
    {
        // Vérifier si le paramètre "Enraged" de l'Animator est vrai
        // ET que le trigger "Attack" n'est pas actif
        if (animator != null && animator.GetBool("Enraged") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            // Appliquer la rotation cible indépendamment de la rotation du parent
            transform.rotation = targetRotation;
        }
    }
}
