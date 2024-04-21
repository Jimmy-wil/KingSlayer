using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetttings : MonoBehaviour
{
   [SerializeField] AudioMixer mixer;
   [SerializeField] Slider musicslider;
   [SerializeField] Slider SFXslider;


   const string Mixermusic = "MusicVolume";
   const string MixerSFX = "SFXVolume";

   /*void Awake()
   {
    musicslider.onValueChanged.AddListener(setmusicvolume);
    SFXslider.onValueChanged.AddListener(setsfxvolume);
   }

   void setmusicvolume(float value)
   {
      mixer.SetFloat(Mixermusic, value);
   }

    void setsfxvolume(float value)
   {
      mixer.SetFloat(MixerSFX, Mathf.Log10(value) * 20);
   }*/

   public void SetMusicvol()
   {
      float volume = musicslider.value;
      mixer.SetFloat("MusicVolume", volume);
   }
}
