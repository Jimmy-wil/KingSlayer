using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class InventoryController : NetworkBehaviour
{
    [SerializeField] 
    private UIInvetoryPage inventoryUI;
   // [SerializeField] private MouseFollower _mouseFollower;

   [SerializeField] private InventorySO inventoryData;
  // [SerializeField] private InventorySO hotbarData;

  [SerializeField] private HotBarController hotbar;
   
    private bool inventoryIsClosed;

    public List<InventoryItem> initialItems = new List<InventoryItem>();
  //  public List<InventoryItem> initialHotbarItems = new List<InventoryItem>();

    
    void Start()
    {
      //inventoryUI =  GameObject.Find("Inventory").GetComponent<UIInvetoryPage>();
      
        PrepareUI();
        PrepareHotbar();
        PrepareInventoryData();
        inventoryIsClosed = false;
    }
    
    

    private void PrepareInventoryData()
    {
        
        inventoryData.Initialize();
        inventoryData.OnInventoryUpdated += UpdateInventoryUI;
        foreach (InventoryItem item in initialItems)
        {
            if (item.IsEmpty)
                continue;
            inventoryData.AddItem(item);
        }
        
        
        //  hotbarData.Initialize();
      //  hotbarData.OnInventoryUpdated += UpdateInventoryUI;
      // foreach (InventoryItem item in initialHotbarItems)// {
      //      if (item.IsEmpty)
      //          continue;
      //      hotbarData.AddItem(item);
      //  }
        
    }

   
    
    
    

    private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
    {
        inventoryUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            inventoryUI.UpdateData(item.Key,item.Value.item.ItemImage, item.Value.quantity);
        }
    }

    private void PrepareUI()
    {
        inventoryUI.InitializeInventoryUI(inventoryData.Size);
        this.inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
        this.inventoryUI.OnSwapItems += HandleSwapItems;
        this.inventoryUI.OnStartDragging += HandleDragging;
        this.inventoryUI.OnItemActionRequested += HandleItemActionRequest;
    }

    private void PrepareHotbar()
    {
        hotbar.InitializeHotbar();
    }

    private void UpdateHotbarUI(Dictionary<int, InventoryItem> inventoryState)
    {
        hotbar.ResetAllHotBarItems();
        foreach (var item in inventoryState)
        {
            hotbar.UpdateDataHotbar(item.Key,item.Value.item.ItemImage, item.Value.quantity);
        }
    }

    private void HandleItemActionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if(inventoryItem.IsEmpty)
            return;
        IItemAction itemAction = inventoryItem.item as IItemAction;
        if (itemAction != null)
        {
            itemAction.PerfomAction(gameObject);
        }
        IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
        if (destroyableItem != null)
        {
            inventoryData.RemoveItem(itemIndex,1);
        }
    }

    private void HandleDragging(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if(inventoryItem.IsEmpty)
            return;
        inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
    }

    private void HandleSwapItems(int itemIndex1, int itemIndex2)
    {
        inventoryData.SwapItems(itemIndex1, itemIndex2);
    }

    private void HandleDescriptionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            inventoryUI.ResetSelection();
            return;
        }
           
        ItemSO item = inventoryItem.item;
        inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
    }


    void Update()
    {
        hotbar.ResetAllHotBarItems();
        int cpt = 0;
        foreach (var item in inventoryData.GetCurrentItemState())
        {
            if(cpt > 3) break;
            hotbar.UpdateDataHotbar(item.Key, item.Value.item.ItemImage,item.Value.quantity);
            cpt++;
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryIsClosed)
            {
                inventoryUI.Show();
                foreach (var item in inventoryData.GetCurrentItemState())
                {
                    inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage,item.Value.quantity);
                }
                inventoryIsClosed = false;
               //  hotbar.UpdateDataHotbar(); //temp
                
            }
            else
            {
                inventoryUI.Hide();
                inventoryIsClosed = true;
              
            }
        }
        // toggle = !toggle;
           // inventoryUI.gameObject.SetActive(toggle);
           // _mouseFollower.gameObject.SetActive(toggle);
          

    }
}
