using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingController : MonoBehaviour
{

    [SerializeField]
    private CraftingPage craftingUI;

    [SerializeField]
    private InventorySO craftingData;
    
    public List<InventoryItem> initialItems = new List<InventoryItem>();
    
    private void Start()
    {
      PrepareCraftingData();
      PrepareUI();
    }


    private void PrepareCraftingData()
    {
        craftingData.Initialize();
        craftingData.OnInventoryUpdated += UpdateCraftingUI;
        foreach (InventoryItem item in initialItems)
        {
            if (item.IsEmpty)
                continue;
            craftingData.AddItem(item);

        }
    }
    
    private void UpdateCraftingUI(Dictionary<int, InventoryItem> inventoryState)
    {
        craftingUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            craftingUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
        }
    }
    
    private void PrepareUI()
    {
        craftingUI.InitializeCraftingUI();
        this.craftingUI.OnDescriptionRequested += HandleDescriptionRequest;
        this.craftingUI.OnSwapItems += HandleSwapItems;
        this.craftingUI.OnStartDragging += HandleDragging;
        this.craftingUI.OnItemActionRequested += HandleItemActionRequest;
    }
    
    private void HandleItemActionRequest(int itemIndex)
    {
        
    }

    private void HandleDragging(int itemIndex)
    {
        InventoryItem inventoryItem = craftingData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
            return;
        craftingUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
    }

    private void HandleSwapItems(int itemIndex1, int itemIndex2)
    {
        craftingData.SwapItems(itemIndex1, itemIndex2);
    }

    private void HandleDescriptionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = craftingData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            craftingUI.ResetSelection();
            return;
        }

        ItemSO item = inventoryItem.item;
        craftingUI.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
    }
    
    

    private void Update()
    {
        foreach (var item in craftingData.GetCurrentItemState())
        {
            craftingUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
        }
    }
}
