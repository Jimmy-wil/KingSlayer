using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionMusic : MonoBehaviour
{
   [SerializeField] Slider volumeslider;

   void Start()
   {
    if(!PlayerPrefs.HasKey("musicVolume"))
    {
        PlayerPrefs.SetFloat("musicVolume", 1);
        Load();
    }

    else 
    {
       Load();
    }

   }

public void changevolme()
{
    AudioListener.volume = volumeslider.value;
    Save();
}

private void Load()
{
    volumeslider.value = PlayerPrefs.GetFloat("musicVolume");
}

private void Save()
{
    PlayerPrefs.SetFloat("musicVolume", volumeslider.value);
}

}
