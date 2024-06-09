using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnraged : StateMachineBehaviour
{
    private WeaponParent weaponParent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        weaponParent = animator.GetComponentInParent<WeaponParent>();

        if (weaponParent != null)
        {
            weaponParent.degat += 20; // Increase attack damage by 10
            weaponParent.delay *= 0.5f; // Decrease attack delay by 50%
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (weaponParent != null)
        {
            weaponParent.degat -= 10; // Reset attack damage to original value
            weaponParent.delay /= 0.5f; // Reset attack delay to original value
        }
    }
}
