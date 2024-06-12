using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour
{
    public UnityEvent OnAnimationEventTriggered, OnAttackPerformed;


    public void TriggerEvent()
    {
        try
        {
            OnAnimationEventTriggered?.Invoke();

        }
        catch 
        {
            Debug.Log("un pb?");
        }
    }
    public void TriggerAttack()
    {
        try
        {
            OnAttackPerformed?.Invoke();

        }
        catch 
        {
            Debug.Log("un pb?!");
        }

    } 
}
