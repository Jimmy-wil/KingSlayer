using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnraged : StateMachineBehaviour
{
    [SerializeField]
    private int degatIncrease = 20;

    [SerializeField]
    private float delayIncrease = 0;

    [SerializeField]
    private float radius = 2;

    [SerializeField]
    private float attackDistance = 5.0f; // Ajouter une variable pour la distance d'attaque
    
    [SerializeField]
    private float newSpeed; // Variable pour la nouvelle vitesse

    private WeaponParent weaponParent;
    private KnockBackFeedBack knockbackComponent;
    private AI aiComponent;
    private AgentMover agentMover;

    
  

    // OnStateEnter est appelé au début de la transition et l'évaluation de cet état
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        weaponParent = animator.GetComponentInParent<WeaponParent>();
        knockbackComponent = animator.GetComponentInParent<KnockBackFeedBack>();
        aiComponent = animator.GetComponentInParent<AI>();
        agentMover = weaponParent.GetComponentInParent<AgentMover>();
       
        if (weaponParent != null)
        {
            weaponParent.degat += degatIncrease; // Augmente les dégâts
            weaponParent.delay += delayIncrease; // Réduit le délai d'attaque
            weaponParent.radius += radius;
        }


        if (aiComponent != null)
        {
            aiComponent.attackDistance += attackDistance; // Appliquer la distance d'attaque
        }

        if (agentMover != null)
        {
            agentMover.maxSpeed = newSpeed; // Appliquer la nouvelle vitesse
        }
        
    }

}
