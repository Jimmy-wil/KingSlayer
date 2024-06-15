using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class CameraShake : MonoBehaviour
{
  
    public GameObject shakeFX;
    public float shakeDur = 0.5f;
    void Start()
    {
        shakeFX.SetActive(false);
    }

    public void Play()
    {
        shakeFX.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(Shake(shakeDur));
    }
   
    IEnumerator Shake(float t)
    {
        yield return new WaitForSeconds(t);
        shakeFX.SetActive(false);
    }

}
