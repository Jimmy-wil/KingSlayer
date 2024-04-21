using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class audioManager : MonoBehaviour
{
    [Header("-----Audio Source-----")]
  [SerializeField] AudioSource musicSource;
   [SerializeField] AudioSource musicSFX;

 [Header("-----Audio Sfx-----")]
   public AudioClip background;
   public AudioClip clip;



private void Start()
{
  musicSource.clip = background;
  
  musicSource.Play();
}

 public void PlaySFX(AudioClip clip)
 {
  musicSFX.PlayOneShot(clip);
 }
}
