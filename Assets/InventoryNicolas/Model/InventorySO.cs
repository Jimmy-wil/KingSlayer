using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    [SerializeField]
    public List<InventoryItem> inventoryItems;

    [field: SerializeField]
    public int Size { get; private set; } = 10;

    public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;  
    
    public void Initialize()
    {
        inventoryItems = new List<InventoryItem>();
        for (int i = 0; i < Size + 6; i++)
        {
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }

    public int AddItem(ItemSO item, int quantity)
    {
        if (item.IsStackable == false)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                while (quantity > 0 && IsInventoryFull() == false)
                {
                   quantity -= AddItemToFirstFreeSlot(item, 1);
                }
                InformAboutChange();
                return quantity;
            }
        }

        quantity = AddStackableItem(item, quantity);
        InformAboutChange();
        return quantity;
    }

    private int AddItemToFirstFreeSlot(ItemSO item, int quantity)
    {
        InventoryItem newItem = new InventoryItem
        {
            item = item,
            quantity = quantity
        };

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
            {
                inventoryItems[i] = newItem;
                return quantity;
            }
        }
        return 0;
    }

    private bool IsInventoryFull() => inventoryItems.Where(item => item.IsEmpty).Any() == false;
   

    private int AddStackableItem(ItemSO item, int quantity)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if(inventoryItems[i].IsEmpty)
                continue;
            if (inventoryItems[i].item.ID == item.ID)
            {
                int amountPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;
                if (quantity > amountPossibleToTake)
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize);
                    quantity -= amountPossibleToTake;
                }
                else
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                    InformAboutChange();
                    return 0;
                }
            }
        }

        while (quantity > 0 && IsInventoryFull() == false)
        {
            int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
            quantity -= newQuantity;
            AddItemToFirstFreeSlot(item, newQuantity);
        }

        return quantity;

    }

    internal void AddItem(InventoryItem item)
    {
        AddItem(item.item, item.quantity);
    }

    public Dictionary<int, InventoryItem> GetCurrentItemState()
    {
        Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
        
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if(inventoryItems[i].IsEmpty)
                continue;
            returnValue[i] = inventoryItems[i];
        }

        return returnValue;
    }

    public InventoryItem GetItemAt(int itemIndex)
    {
        return inventoryItems[itemIndex];
    }

    internal void SwapItems(int itemIndex1, int itemIndex2)
    {
        InventoryItem item1 = inventoryItems[itemIndex1];
        inventoryItems[itemIndex1] = inventoryItems[itemIndex2];
        inventoryItems[itemIndex2] = item1;
        InformAboutChange();
    }

    public void InformAboutChange()
    {
        OnInventoryUpdated?.Invoke(GetCurrentItemState());
    }

    public void RemoveItem(int itemIndex, int amount)
    {
        if (inventoryItems.Count > itemIndex)
        {
            if (inventoryItems[itemIndex].IsEmpty)
                return;
            int reminder = inventoryItems[itemIndex].quantity - amount;
            if (reminder <= 0)
                inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
            else
                inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(reminder);
            InformAboutChange();


        }
    }

    public void RemoveAtAllCost(int itemIndex, int amount)
    {
        int reminder = inventoryItems[itemIndex].quantity - amount;
        Debug.Log(reminder);
        Debug.Log(amount);
        if (reminder <= 0)
        {
           inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
            Debug.Log("TESSSSSSSSSSSSSSSSSTTTTTT");
        }
           
        else
            inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(reminder);
        InformAboutChange();
        
    }

    public void RemoveItem(int itemIndex)
    {
        if (inventoryItems[itemIndex].IsEmpty)
            return;
        inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
        InformAboutChange();
        
    }
}

[Serializable]
public struct InventoryItem 
{
    public int quantity;
    public ItemSO item;
    
   // private InventoryController inventoryController =>  GameObject.Find("InventoryMenuUI").GetComponent<InventoryController>();
    
  

    public InventoryItem(int quantity, ItemSO item)
    {
        this.quantity = quantity;
        this.item = item;
        
    }
    

    public bool IsEmpty => item == null;

    public InventoryItem ChangeQuantity(int newQuantity)
    {
        return new InventoryItem
        {
            item = this.item,
            quantity = newQuantity
        };

    }

    public static InventoryItem GetEmptyItem()
        => new InventoryItem
        {
            item = null,
            quantity = 0,
        };
}
