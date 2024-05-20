using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : NetworkBehaviour
{
    private AgentAnimation agentAnimations;
    private AgentMover agentMover;

   

    private Vector2 pointerInput, movementInput;

    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

    private WeaponParent weaponParent;

  
    

    public void PerformAttack()
    {
        weaponParent.Attack();
    }

    private void Awake()
    {
        agentAnimations = GetComponentInChildren<AgentAnimation>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        agentMover = GetComponent<AgentMover>();
    }

    private void AnimateCharacter()
    {
        Vector2 lookDirection = pointerInput - (Vector2)transform.position;
        agentAnimations.RotateToPointer(lookDirection);
        agentAnimations.PlayAnimation(movementInput);
      
    }

    private void Update()
    {
        //pointerInput = GetPointerInput();

        // movementInput = movement.action.ReadValue<Vector2>().normalized;
        if (!IsOwner) return;
        agentMover.MovementInput = movementInput;
        weaponParent.Pointerposition = pointerInput;
        AnimateCharacter();

    }

    
}
