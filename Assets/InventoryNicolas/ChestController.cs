using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public GameObject ChestClose,ChestOpen;

    [SerializeField] private InventoryController controller => GameObject.Find("InventoryMenuUI").GetComponent<InventoryController>();
    [SerializeField] private InventorySO chestData;
    [SerializeField] private ChestInventoryPage chestUI;
    
    
    
    public List<InventoryItem> initialItems = new List<InventoryItem>();
    // Start is called before the first frame update
    void Start()
    {
        
       ChestClose.SetActive(true); 
       ChestOpen.SetActive(false);
       PrepareUI();
       PrepareInventoryData();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in chestData.GetCurrentItemState())
        {
            chestUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
        }
    }
    
    private void PrepareUI()
    {
        chestUI.InitializeInventoryUI(chestData.Size);
        this.chestUI.OnItemActionRequested += HandleItemActionRequest;
    }

    private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
    {
        chestUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            chestUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
        }
    }
    
    private void PrepareInventoryData()
    {
        chestData.Initialize();
        chestData.OnInventoryUpdated += UpdateInventoryUI;
        foreach (InventoryItem item in initialItems)
        {
            if (item.IsEmpty)
                continue;
            chestData.AddItem(item);

        }
    }

  
    
    private void HandleItemActionRequest(int itemIndex)
    {
        Debug.Log("click");
        InventoryItem inventoryItem = chestData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
            return;
        
        controller.inventoryData.AddItem(chestData.GetItemAt(itemIndex));
       
      // IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
     //  if (destroyableItem != null)
     //  {
           chestData.RemoveItem(itemIndex, initialItems[itemIndex].quantity );
      // }
    }
    
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ChestClose.SetActive(false); 
        ChestOpen.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ChestClose.SetActive(true);
        ChestOpen.SetActive(false);
    }
}
