using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] 
    private UIInvetoryPage inventoryUI;
   // [SerializeField] private MouseFollower _mouseFollower;

   [SerializeField] private InventorySO inventoryData;
   
    private bool inventoryIsClosed;

    public List<InventoryItem> initialItems = new List<InventoryItem>();

    
    void Start()
    {
        PrepareUI();
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

    private void HandleItemActionRequest(int itemIndex)
    {
       
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
            }
            else
            {
                inventoryUI.Hide();
                inventoryIsClosed = true;
            }
            
           // toggle = !toggle;
           // inventoryUI.gameObject.SetActive(toggle);
           // _mouseFollower.gameObject.SetActive(toggle);
        }
    }
}
