using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class audioManager : MonoBehaviour
{
    [Header("-----Audio Source-----")]
  
   [SerializeField] AudioSource musicSFX;

 [Header("-----Audio Sfx-----")]
   public AudioClip background;
   public AudioClip clip;





 public void PlaySFX(AudioClip clip)
 {
  musicSFX.PlayOneShot(clip);
 }
}
