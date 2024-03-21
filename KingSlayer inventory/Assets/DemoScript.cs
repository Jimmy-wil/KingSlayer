using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
  public InventoryManager inventoryManager;
  public Itemscript[] itemstopickup;

  public void PickupItem(int id)
  {
    bool result = inventoryManager.AddItem(itemstopickup[id]);
    if (result == true)
    {
      Debug.Log("Item added");
      
    }
    else
    {
      Debug.Log("ERROR FAILED TO ADD ITEM");
    }
  }
}
