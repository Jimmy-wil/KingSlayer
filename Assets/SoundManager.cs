using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
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
        if (!musicslid) return;

        float volume = musicslid.value;
        MyMixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVol", volume);
    }

     public void setsfxvolume()
    {
        if (!sfxslid) return;

        float volume = sfxslid.value;
        MyMixer.SetFloat("SFX", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("sfxVol", volume);
    }


    private void Loadvolume()
    {
        if (musicslid)
        {
            musicslid.value = PlayerPrefs.GetFloat("musicVol");

        }
        if(sfxslid)
        {
            sfxslid.value = PlayerPrefs.GetFloat("sfxVol");

        }
        setmusicvolume();
        setsfxvolume();
       
    }
}
