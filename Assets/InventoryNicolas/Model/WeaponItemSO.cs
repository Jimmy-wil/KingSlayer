using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class WeaponItemSO : ItemSO
{
    public WeaponType type;
    public int weaponDmg;
    public float range;
    public int key;

    public bool PerformAction(GameObject character)
    {
        return true;
    }
    
    

}
