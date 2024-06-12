using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class InventoryController : NetworkBehaviour
{
    [SerializeField]
    public UserDataScript UserData;

    [SerializeField]
    public GameObject player;

    [SerializeField]
    private UIInvetoryPage inventoryUI;

    // [SerializeField] private MouseFollower _mouseFollower;

    [SerializeField] public InventorySO inventoryData;

    [SerializeField]
    private HotBarController hotbar;

    public bool inventoryIsClosed;

    public List<InventoryItem> initialItems = new List<InventoryItem>();

    void Start()
    {
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
            hotbar.UpdateDataHotbar(item.Key, item.Value.item.ItemImage, item.Value.quantity);
        }
    }

    private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
    {
        inventoryUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
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
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
            return;
        IItemAction itemAction = inventoryItem.item as IItemAction;
        if (itemAction != null)
        {
            player = GameObject.Find(UserData.Username);
            if (player != null)
            {
                Debug.Log("Performing action");
                itemAction.PerfomAction(player);
                player = null;

            }
        }
        IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
        if (destroyableItem != null)
        {
            inventoryData.RemoveItem(itemIndex, 1);
        }
    }

    private void HandleDragging(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
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
            if (cpt > 3) break;
            hotbar.UpdateDataHotbar(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            cpt++;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryIsClosed)
            {
                inventoryUI.Show();
                foreach (var item in inventoryData.GetCurrentItemState())
                {
                    inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
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
    }
}
