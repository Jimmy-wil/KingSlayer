using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Agent : NetworkBehaviour
{
    private AgentAnimation agentAnimations;
    private AgentMover agentMover;

    private int previousChildCount;
    
    private Vector2 pointerInput, movementInput;

    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

    private WeaponInterface weaponParent;

  
    

    public void PerformAttack()
    {
        if (weaponParent == null)
        {
            Debug.Log("No Weapon parent!");
        }
        else
        {
            weaponParent.Attack();

        }
    }

    private void Awake()
    {
        agentAnimations = GetComponentInChildren<AgentAnimation>();
        weaponParent = GetComponentInChildren<WeaponInterface>();
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

        PreviousChildCountUpdate();

        agentMover.MovementInput = movementInput;
        if(weaponParent != null)
        {
            weaponParent.Pointerposition = pointerInput;
        }
        AnimateCharacter();

    }

    private void PreviousChildCountUpdate()
    {
        if (transform.childCount != previousChildCount)
        {
            previousChildCount = transform.childCount;
            weaponParent = GetComponentInChildren<WeaponInterface>();
        }
        else if (transform.childCount < previousChildCount)
        {
            previousChildCount = transform.childCount;
        }
    }


}
