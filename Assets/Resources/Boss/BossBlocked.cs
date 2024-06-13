using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossBlocked : MonoBehaviour
{
   [SerializeField]
    private Rigidbody2D rb2d;

    [SerializeField]
    private float delay = 0.15f;
    
    public UnityEvent OnBegin, OnDone;

   
    public void PlayFeedback(GameObject sender)
    {
        
        StopAllCoroutines();
        OnBegin?.Invoke();
        StartCoroutine(Reset());
        
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        OnDone?.Invoke();
    }
}
