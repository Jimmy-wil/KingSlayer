using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeWeaponBehavior : StateMachineBehaviour
{

    private Transform weaponTransform;
    private Transform weaponParentTransform;
    private Quaternion originalParentRotation;

    // OnStateEnter est appelé lorsqu'une transition commence et que le state machine commence à évaluer ce state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        weaponTransform = animator.transform; // Assurez-vous que le Weapon est sur le même GameObject que l'Animator
        weaponParentTransform = weaponTransform.parent; // Cibler le parent du transform

        if (weaponParentTransform != null)
        {
            originalParentRotation = weaponParentTransform.rotation; // Sauvegarder la rotation initiale du parent
        }

        // Bloquer la rotation sur l'axe X de Weapon à 0
        Quaternion targetRotation = Quaternion.Euler(0, weaponTransform.localRotation.eulerAngles.y, weaponTransform.localRotation.eulerAngles.z);
        weaponTransform.localRotation = targetRotation;
    }

    // OnStateUpdate est appelé à chaque frame Update entre OnStateEnter et OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Déplacer l'arme sur l'axe X d'un certain nombre donné
        if (weaponTransform != null)
        {
            Vector3 newPosition = weaponTransform.position;
            
            weaponTransform.position = newPosition;
        }

        // Bloquer la rotation du parent
        if (weaponParentTransform != null)
        {
            weaponParentTransform.rotation = originalParentRotation;
        }

        // Vérifier si la rotation sur l'axe X atteint -180 degrés
        if (Mathf.Abs(weaponTransform.localRotation.eulerAngles.x + 180f) < 1f)
        {
            // Ajuster la rotation sur l'axe X à 0 degrés et sur l'axe Z à 360 degrés
            Quaternion newRotation = Quaternion.Euler(0, weaponTransform.localRotation.eulerAngles.y, 360f);
            weaponTransform.localRotation = newRotation;
        }
    }

    // OnStateExit est appelé lorsque une transition se termine et que le state machine arrête d'évaluer ce state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Optionnel: Restaurer la rotation si nécessaire
        // if (weaponParentTransform != null)
        // {
        //     weaponParentTransform.rotation = originalParentRotation;
        // }
    }
}
