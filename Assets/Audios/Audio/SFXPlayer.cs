using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
   AudioSource aud;
    void Start()
    {
        aud =GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play()
    {
        aud.Play();
    }

    public void Stop()
    {
        aud.Stop();
    }

}
