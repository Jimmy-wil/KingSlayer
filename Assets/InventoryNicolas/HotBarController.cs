using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class HotBarController : MonoBehaviour
{
 [SerializeField] public InventoryController Controller;

    [SerializeField] public UIInvetoryPage inventoryData;

    [SerializeField] private GameObject player;

    [SerializeField] public RectTransform hotbarUI;

    public List<UIInventoryItem> listOfUIItemsHotbar = new List<UIInventoryItem>();

   // private bool Itemselected = false;

   public InventoryItem SelectedItem;
   public InventoryItem selectedItem;
   
   
    private void Update()
    {
        // faire 3 cas pour eviter le indexOutofrange => 1 item, 2 item et 3 item !!!!!!
       
        if (Input.GetKeyDown("1"))
        {
           DeselectAll();
           listOfUIItemsHotbar[0].Select();
           SelectedItem = Controller.initialItems[0];
           Debug.Log("1");
        }

        if (Input.GetKeyDown("2"))
        {
            DeselectAll(); 
           listOfUIItemsHotbar[1].Select();
            Debug.Log("2");
            SelectedItem = Controller.initialItems[1];
        }

        if (Input.GetKeyDown("3"))
        {
            DeselectAll();
            listOfUIItemsHotbar[2].Select();
            Debug.Log("3");
            SelectedItem = Controller.initialItems[2];
        }

        if (Controller.inventoryIsClosed && Input.GetKeyDown(KeyCode.Space))
        {
            ItemAction();
            Debug.Log("Is perfoming Action");
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

    public void ItemAction()
    {
        player = GameObject.Find(Controller.UserData.Username);
        if (player != null)
        {
            Debug.LogWarning("Player Not found");
            return;
        }
        

        if (SelectedItem.item is ConsumableItemSO  consumable)
        {
            consumable.PerfomAction(player);
        }

        if (SelectedItem.item is WeaponItemSO weapon && weapon.type is ItemSO.WeaponType.Axe)
        {
            weapon.PerformAction(player);
        }
        
        player = null;
    }
}