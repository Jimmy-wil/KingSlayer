using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxStackedItems = 9;
    public Inventoryslot[] InventorySlots;
    public GameObject inventoryItemPrefab;
    public bool AddItem(Itemscript item)
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            Inventoryslot slot = InventorySlots[i];
            Dragscript itemInSlot = slot.GetComponentInChildren<Dragscript>();
            if (itemInSlot != null && itemInSlot.item == item &&  itemInSlot.count < maxStackedItems )
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        
        
        
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            Inventoryslot slot = InventorySlots[i];
            Dragscript itemInSlot = slot.GetComponentInChildren<Dragscript>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item,slot);
                return true;
            }
        }

        return false;
    } 

    void SpawnNewItem(Itemscript item, Inventoryslot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        Dragscript inventoryItem = newItemGo.GetComponent<Dragscript>();
        inventoryItem.InitialiseItem(item);
    }
}
