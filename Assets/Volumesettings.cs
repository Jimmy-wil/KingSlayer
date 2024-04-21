using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Volumesettings : MonoBehaviour
{
   [SerializeField] private AudioMixer myMixer;
   [SerializeField] private Slider musicslider;

private void start()
{
    setmusicvolume();
}
   public void setmusicvolume()
   {
    float volume = musicslider.value;
    myMixer.SetFloat("music", MathF.Log10(volume) * 20 );
   }
}
