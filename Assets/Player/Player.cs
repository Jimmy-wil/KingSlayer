using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private AgentAnimation agentAnimations;
    private AgentMover agentMover;

    [SerializeField]
    private InputActionReference movement,attack,pointerPosition;

    private Vector2 pointerInput, movementInput;

    private WeaponParent weaponParent;

  
    private void OnEnable()
    {
        attack.action.performed+=PerformAttack;
    }

    private void OnDisable()
    {
        attack.action.performed-=PerformAttack;
    }

    private void PerformAttack(InputAction.CallbackContext obj)
    {
        weaponParent.Attack();
    }

    private void Awake()
    {
        agentAnimations = GetComponentInChildren<AgentAnimation>();
        weaponParent=GetComponentInChildren<WeaponParent>();
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
        pointerInput=GetPointerInput();

        weaponParent.Pointerposition=pointerInput;

        movementInput=movement.action.ReadValue<Vector2>().normalized;

        agentMover.MovementInput =movementInput;

        AnimateCharacter();

    }

    private Vector2 GetPointerInput()
    {
    Vector3 mousPos =pointerPosition.action.ReadValue<Vector2>();
    mousPos.z =Camera.main.nearClipPlane;
    return Camera.main.ScreenToWorldPoint(mousPos);
    }
}
