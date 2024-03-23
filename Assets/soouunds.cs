using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class soouunds : MonoBehaviour
{
    [SerializeField] Image soundonicon;
      [SerializeField] Image soundoff;
      private bool muted = false;

    
    
    void Start()
    {
        if (!PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("muted", 0);
            load();
        }
        else 
        {
            load();
        }
        updatebuttonicon();
        AudioListener.pause = muted;
    }

public void onbutonpress(){
    if (muted == false)
    {
        muted = true;
        AudioListener.pause = true;
    }
    else 
    {
        muted = false;
        AudioListener.pause = false;
    }
    save();
    updatebuttonicon();
}

private void updatebuttonicon(){
    if (muted == false)
    {
        soundonicon.enabled = true;
        soundoff.enabled = false;

    }
    else 
    {
         soundonicon.enabled = false;
        soundoff.enabled = true;
    }
}
   
   private void load(){
    muted = PlayerPrefs.GetInt("muted") == 1;
   }

   private void save(){
    PlayerPrefs.SetInt("muted", muted ? 1 : 0);
   }
}
