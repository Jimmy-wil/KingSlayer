using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBarController : MonoBehaviour
{
 //[SerializeField] public InventoryController Controller;

    [SerializeField] public UIInvetoryPage inventoryData;

    [SerializeField] public RectTransform hotbarUI;

    public List<UIInventoryItem> listOfUIItemsHotbar = new List<UIInventoryItem>();

   // private bool Itemselected = false;

    private void Update()
    {
       
        if (Input.GetKeyDown("1"))
        {
           DeselectAll();
           listOfUIItemsHotbar[0].Select();
            Debug.Log("1");
        }

        if (Input.GetKeyDown("2"))
        {
            DeselectAll(); 
           listOfUIItemsHotbar[1].Select();
            Debug.Log("2");
        }

        if (Input.GetKeyDown("3"))
        {
            DeselectAll();
            listOfUIItemsHotbar[2].Select();
            Debug.Log("3");
        }
    }


    public void InitializeHotbar()
    {
        for (int i = 0; i < 3; i++)
        {   
            UIInventoryItem uiItem = Instantiate(inventoryData.itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(hotbarUI);
             listOfUIItemsHotbar.Add(uiItem);
           
        }
    }

    public void UpdateDataHotbar(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (listOfUIItemsHotbar.Count> itemIndex)
        {
            listOfUIItemsHotbar[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    public void ResetAllHotBarItems()
    {
        foreach (var item in listOfUIItemsHotbar)
        {
            item.ResetData();
            
        }
    }

    public void DeselectAll()
    {
        foreach (var item in listOfUIItemsHotbar)
        {
            item.Deselect();
        }
    }
}