using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class craftingSolution : MonoBehaviour
{
    public bool Alleluja { get; set; }
    public bool Exist { get; set; }

    [SerializeField]
    public GameObject thisgameobject;
    
    private void Update()
    {
       
        
        
      if (thisgameobject.transform.Find("ItemUI(Clone)").transform.Find("Border").transform.Find("Image").gameObject
          == null)
        {
           Exist = false;
           return;
        }

         if ( thisgameobject.transform.Find("ItemUI(Clone)").transform.Find("Border").transform.Find("Image").gameObject.activeSelf)
         {
          Debug.Log("ca marche");
         Alleluja = false;
          Exist = true; }

        
         if(thisgameobject.transform.Find("ItemUI(Clone)").transform.Find("Border").transform.Find("Image").gameObject.activeSelf == false)
         { Exist = true;
           Alleluja = true;Debug.Log("ca marche youpiiiiiiuiuyuiyuiyuiyuiy");
          }
    }

    public void Status()
    {
        if ( thisgameobject.transform.Find("ItemUI(Clone)").transform.Find("Border").transform.Find("Image").gameObject.activeSelf)
        {
            
            Alleluja = false;
            Debug.Log("ca marche");
        }
        else
        {
            Alleluja = true;
            Debug.Log("ca marche youpiiiiiiuiuyuiyuiyuiyuiy");
        }

    }
}
