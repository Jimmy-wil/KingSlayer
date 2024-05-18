using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
   //Script a attacher au joueur pour qu il fonctionne !!!

   [SerializeField] private UIInvetoryPage inventoryUI;

   public int inventorySize = 10;
    void Start()
   {
      inventoryUI.InitializeInventoryUI(inventorySize);
   }


    void Update()
   {
      if (Input.GetKeyDown(KeyCode.I) == false)
      {
         inventoryUI.Show();
      }
      else
      {
         inventoryUI.Hide();
      }
   }
}
