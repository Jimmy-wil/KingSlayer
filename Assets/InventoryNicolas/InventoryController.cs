using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
   //Script a attacher au joueur pour qu il fonctionne !!!

   [SerializeField] private UIInvetoryPage inventoryUI;
   [SerializeField] private InventorySO inventoryData;
   public int inventorySize = 10;
   public bool inventoryIsClosed;
    void Start()
   {
      inventoryUI.InitializeInventoryUI(inventorySize);
      //inventoryData.Initialize();
      inventoryIsClosed = false;
   }


    void Update()
   {
      if (Input.GetKeyDown(KeyCode.I))
      {
         if (inventoryIsClosed == true)
         {
            inventoryUI.Show();
           // foreach (var item in inventoryData.GetCurrentItemState())
           // {
           //    inventoryUI.UpdateData(item.Key,item.Value.item.ItemImage, item.Value.quantity);
          //  }

            inventoryIsClosed = false;
         }
         else
         {
            inventoryUI.Hide();
            inventoryIsClosed = true;
         }
        
      }
     
   }
   

   
    
    
    
    
    
}
