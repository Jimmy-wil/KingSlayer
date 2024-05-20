using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Volsetts : MonoBehaviour
{
    [SerializeField] private AudioMixer MyMixer;
    [SerializeField] private Slider musicslid;
    [SerializeField] private Slider sfxslid;

    
 private void Start()
 {
   if(PlayerPrefs.HasKey("musicVol"))
   {
    Loadvolume();
   }
   else 
   {
     setmusicvolume();
     setsfxvolume();
   }
   
 }

    public void setmusicvolume()
    {
        float volume = musicslid.value;
        MyMixer.SetFloat("music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVol", volume);
    }

     public void setsfxvolume()
    {
        float volume = sfxslid.value;
        MyMixer.SetFloat("SFX", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("sfxVol", volume);
    }


    private void Loadvolume()
    {
        musicslid.value = PlayerPrefs.GetFloat("musicVol");
        sfxslid.value = PlayerPrefs.GetFloat("sfxVol");

        setmusicvolume();
        setsfxvolume();
    }

    
}
