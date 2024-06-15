using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class craftingSolution : MonoBehaviour
{
    public bool Alleluja { get; set; }
    public bool Exist { get; set; }

    private void Update()
    {
        var find1 = this.gameObject.transform.Find("ItemUI(Clone)");
        var find2 = find1 != null ? find1.transform.Find("Border") : null;
        var find3 = find2 != null ? find2.transform.Find("Image").gameObject : null;


        if (find3 == null)
        {
            Exist = false;
            return;
        }

        if (find3.activeSelf)
        {
            Alleluja = false;
            Exist = true; 
        }
        else
        {
            Exist = true;
            Alleluja = true;
        }

    }

    public void Status()
    {
        var find1 = this.gameObject.transform.Find("ItemUI(Clone)");
        var find2 = find1 != null ? find1.transform.Find("Border") : null;
        var find3 = find2 != null ? find2.transform.Find("Image").gameObject : null;

        if (find3.activeSelf)
        {
            Alleluja = false;
        }
        else
        {
            Alleluja = true;
        }

    }
}
