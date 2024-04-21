using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
   public UnityEvent<Vector2> OnMovementInput,OnPointerInput;
   public UnityEvent OnAttack;

    [SerializeField]
    private InputActionReference movement,attack,pointerPosition;

    public void Update()
    {
        OnMovementInput?.Invoke(movement.action.ReadValue<Vector2>().normalized);
        OnPointerInput?.Invoke(GetPointerInput());
    }

    private Vector2 GetPointerInput()
    {
    Vector3 mousPos =pointerPosition.action.ReadValue<Vector2>();
    mousPos.z =Camera.main.nearClipPlane;
    return Camera.main.ScreenToWorldPoint(mousPos);
    }

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
        OnAttack?.Invoke();
    }
}
