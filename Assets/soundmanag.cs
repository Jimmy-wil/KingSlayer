using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class soundmanag : MonoBehaviour
{
    [SerializeField] Slider volumeslider;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicvolume"))
        {
            PlayerPrefs.SetFloat("musicvolume", 1);
        }
        else 
        {
            load();
        }
    }

    public void changevolume(){
        AudioListener.volume = volumeslider.value;
        save();
    }


  private void load(){
    volumeslider.value = PlayerPrefs.GetFloat("musicvolume");
  }
   

   private void save(){
    PlayerPrefs.SetFloat("musicVolume", volumeslider.value);
   }
}
