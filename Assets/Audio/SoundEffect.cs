using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioSource src;
    public AudioClip run,attack;

    public void Run ()
    {
        src.clip=run;
        src.Play();
    }

    public void Attack()
    {
        src.clip=attack;
        src.Play();
    }
    
}
