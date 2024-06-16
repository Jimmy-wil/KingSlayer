using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
     [SerializeField]
    public UserDataScript UserData;

    [SerializeField]
    public GameObject player;

    [SerializeField]
    private UIInvetoryPage inventoryUI;

    [SerializeField] private Vector3 spawnOffset = new Vector3(5, 0, 0);

    

    // [SerializeField] private MouseFollower _mouseFollower;

    [SerializeField] public InventorySO inventoryData;

    [SerializeField]
    private HotBarController hotbar;

    [SerializeField] private craftingSolution Solution;

    [SerializeField] private Item itemDrop;
    [SerializeField] private GameObject ItemSpawnPrefab;
    
    public bool inventoryIsClosed;

    public List<InventoryItem> initialItems = new List<InventoryItem>();
    
    
    
    bool iscrafted = false;

    private UIInventoryItem hoveredItem;

    public int len;
    private int amount;

    void Start()
    {
      
        PrepareUI();
        PrepareHotbar();
        PrepareInventoryData();
        inventoryIsClosed = false;
        len = inventoryUI.listOfUIItems.Count;



    }

    private void PrepareInventoryData()
    {
        inventoryData.Initialize();
        inventoryData.OnInventoryUpdated += UpdateInventoryUI;
        inventoryData.OnInventoryUpdated += UpdateHotbarUI; //
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
    

    private void CraftItem()
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(inventoryUI.listOfUIItems.Count -1);
        InventoryItem inventoryItem2 = inventoryData.GetItemAt(inventoryUI.listOfUIItems.Count -2);
        int quant = GetCraftedAmount(inventoryItem, inventoryItem2);
      //  Debug.Log(quant);
        amount = GetCraftedAmount(inventoryItem, inventoryItem2);
        
        if ( inventoryItem.item is CraftableItemSO craft1 && 
            inventoryItem2.item is CraftableItemSO craft2)
        {
           
            
            
            for (int i = 0; i < craft1.recipes.Count; i++)
            {
                if (craft2.type == craft1.recipes[i].type)
                {
                    PrepareCraftOutput();
                  inventoryData.inventoryItems[initialItems.Count] =
                      new InventoryItem(quant, craft1.craftedItem[i]);
                  inventoryData.InformAboutChange();
                    inventoryUI.UpdateData(initialItems.Count,
                        inventoryData.GetCurrentItemState()[initialItems.Count].item.ItemImage, 
                        inventoryData.GetCurrentItemState()[initialItems.Count].quantity);
                    foreach (var item in inventoryData.GetCurrentItemState())
                    {
                        inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                    }
                   // inventoryData.Initialize();
                    inventoryData.OnInventoryUpdated += UpdateInventoryUI;
                }
            }

            InventoryItem inventoryItemCraft = inventoryData.GetItemAt(inventoryUI.listOfUIItems.Count -1);

           // inventoryUI.listOfUIItems[inventoryUI.listOfUIItems.Count -1].itemImage.gameObject.SetActive(true);

           var lastUIItem = inventoryUI.listOfUIItems[inventoryUI.listOfUIItems.Count - 1];
           
         //  if (len < inventoryUI.listOfUIItems.Count && Solution.Alleluja && Solution.Exist)
         inventoryUI.CanCraft = true;
         
         
         
         
         
         return;
         
           
        }
        
        

        InventoryItem inventoryItemRes = inventoryData.GetItemAt(inventoryUI.listOfUIItems.Count -2);
        InventoryItem inventoryItemRes2 = inventoryData.GetItemAt(inventoryUI.listOfUIItems.Count -3);
        
        
        if(inventoryUI.CanCraft && Solution.Alleluja)
        {
            int quant2 = GetCraftedAmount(inventoryItemRes, inventoryItemRes2);
            int index1 = inventoryUI.listOfUIItems.Count - 2;
            int index2 = inventoryUI.listOfUIItems.Count - 3;
           inventoryData.RemoveAtAllCost(index1, quant2);
            inventoryData.RemoveAtAllCost(index2,quant2);
          
       }
        
        
        
        
        
        
       

        
        if(inventoryUI.CanCraft && (inventoryItemRes.item is not CraftableItemSO || inventoryItemRes2.item is not CraftableItemSO))
        {
          
          inventoryUI.DeleteLastItem();
          inventoryUI.CanCraft = false;
          return;
          
        }
    }

   private int GetCraftedAmount(InventoryItem item1, InventoryItem item2)
   {
      
       if (item1.quantity > item2.quantity)
       {
           return item2.quantity;
       }

       if (item1.quantity < item2.quantity)
       {
           return item1.quantity;
       }

       else
       {
           return item1.quantity;
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

    private void PrepareCraftOutput()
    {
        inventoryUI.InitializeCraftResult(); 
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
      //  
        
        
        if (inventoryData.GetItemAt(itemIndex1).IsEmpty || inventoryData.GetItemAt(itemIndex1).IsEmpty)
        {
            inventoryData.SwapItems(itemIndex1, itemIndex2);
            return;
            
        }

        if (inventoryData.GetItemAt(itemIndex1).item == inventoryData.GetItemAt(itemIndex2).item &&
            inventoryData.GetItemAt(itemIndex1).item.IsStackable && inventoryData.GetItemAt(itemIndex2).item.IsStackable) 
        {
          

            inventoryData.AddItem(inventoryData.GetItemAt(itemIndex2).item,
                inventoryData.GetItemAt(itemIndex1).quantity);
             inventoryData.RemoveItem(itemIndex1,inventoryData.GetItemAt(itemIndex1).quantity);
             inventoryData.InformAboutChange();
          inventoryData.SwapItems(itemIndex1, itemIndex2);
        } 
        inventoryData.SwapItems(itemIndex1, itemIndex2);
       
        (initialItems[itemIndex1], initialItems[itemIndex2]) = (initialItems[itemIndex2], initialItems[itemIndex1]);
        
       
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


    public void SetHoveredItem(UIInventoryItem givenItem)
    {
        hoveredItem = givenItem;
    }

    private void SpawnItem()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is not set.");
            return;
        }

        Vector3 spawnPosition = player.transform.position + spawnOffset;
        

        Instantiate(ItemSpawnPrefab, spawnPosition, Quaternion.identity);
        
        

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
        
        CraftItem();
       
        
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
        
     //   if(Input.GetKeyDown(KeyCode.F))
    //    {
     //       if (hoveredItem != null && hoveredItem.IsHovered )
     //       {
    //            Debug.Log(" Ca marche");
    //            SpawnItem();
     //       }
    //    }  
    if (Input.GetKeyDown("1"))
    {
       hotbar.GettingPlayerAndCall();

       hotbar.DeselectAll();

        Debug.Log("1");

        hotbar.listOfUIItemsHotbar[0].Select();
        hotbar.SelectedItem = initialItems[0];
            

        hotbar.ItemAction();
       // inventoryData.InformAboutChange();

    }

    if (Input.GetKeyDown("2"))
    {
        hotbar.GettingPlayerAndCall();

        hotbar.DeselectAll();

        Debug.Log("2");

        hotbar.listOfUIItemsHotbar[1].Select();
        hotbar.SelectedItem = initialItems[1];

        hotbar.ItemAction();
     //   inventoryData.InformAboutChange();

    }

    if (Input.GetKeyDown("3"))
    {
        hotbar.GettingPlayerAndCall();

        hotbar.DeselectAll();

        Debug.Log("3");

        hotbar.listOfUIItemsHotbar[2].Select();
        hotbar.SelectedItem = initialItems[2];

        hotbar.ItemAction();
      //  inventoryData.InformAboutChange();

    }
    
    
    

      
        
            
    
        
        
        
        
    }
}

