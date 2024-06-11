using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Itemworld : MonoBehaviour
{

    public static Itemworld SpawnItemworld(Vector3 position, ItemSO item)
    {
        Transform transform = Instantiate(item.pfItemWorld, position, Quaternion.identity);
       Itemworld itemworld = transform.GetComponent<Itemworld>();
       itemworld.SetItem(item);

       return itemworld;

    }
    
    private ItemSO _item;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void SetItem(ItemSO item)
    {
        this._item = item;
        spriteRenderer.sprite = item.ItemImage;
        
    }
}
