using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HotBarController : MonoBehaviour
{
    [SerializeField] private GameObjectDictionary GameObjectDictionary;

    [SerializeField] public InventoryController Controller;

    [SerializeField] public UIInvetoryPage inventoryData;

    [SerializeField] private GameObject player;

    [SerializeField] public RectTransform hotbarUI;

    [SerializeField] private SpawnWeaponHandlerScript spawnWeaponHandlerScript;

    public List<UIInventoryItem> listOfUIItemsHotbar = new List<UIInventoryItem>();

    // private bool Itemselected = false;

    public InventoryItem SelectedItem;
    private int index;

    private void Update()
    {
        // faire 3 cas pour eviter le indexOutofrange => 1 item, 2 item et 3 item !!!!!!

        if (Input.GetKeyDown("1"))
        {
            index = 0;
        }

        if (Input.GetKeyDown("2"))
        {
            index = 1;

        }

        if (Input.GetKeyDown("3"))
        {
            index = 2;

        }

        if (SelectedItem.item is ConsumableItemSO && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            ItemAction();
        }
        
        

    }

    //  private int NumberOfItemSlots()


    public void GettingPlayerAndCall()
    {
        Debug.Log("GettingPlayerAndCall");
        player = GameObject.Find(Controller.UserData.Username);
        if (player == null)
        {
            Debug.LogWarning("Player Not found");
            return;
        }

        spawnWeaponHandlerScript.DestroyCurrentWeapon(player.GetComponent<NetworkObject>());
        player = null;
    }


    public void InitializeHotbar()
    {
        for (int i = 0; i < 3; i++)
        {
            UIInventoryItem uiItem = Instantiate(inventoryData.itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(hotbarUI);
            listOfUIItemsHotbar.Add(uiItem);

        }
    }

    public void UpdateDataHotbar(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (listOfUIItemsHotbar.Count > itemIndex)
        {
            listOfUIItemsHotbar[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    public void ResetAllHotBarItems()
    {
        foreach (var item in listOfUIItemsHotbar)
        {
            item.ResetData();

        }
    }

    public void DeselectAll()
    {
        foreach (var item in listOfUIItemsHotbar)
        {
            item.Deselect();
        }
    }

    public void ItemAction()
    {
        player = GameObject.Find(Controller.UserData.Username);
        if (player == null)
        {
            Debug.LogWarning("Player Not found");
            return;
        }

        if (SelectedItem.item is ConsumableItemSO consumable)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {





                IItemAction itemAction = SelectedItem.item as IItemAction;
                if (itemAction != null)
                {
                    player = GameObject.Find(Controller.UserData.Username);
                    if (player != null)
                    {
                        Debug.Log("Performing action");
                        itemAction.PerfomAction(player);
                        player = null;



                    }
                }

                IDestroyableItem destroyableItem = SelectedItem.item as IDestroyableItem;
                if (destroyableItem != null)
                {
                    Controller.inventoryData.RemoveItem(index, 1);
                }

            }

            //consumable.PerfomAction(player);
        }
        
        
        
        
        
        if (SelectedItem.item is WeaponItemSO weapon)
        {
                spawnWeaponHandlerScript.SpawnWeapon(weapon.key, player.GetComponent<NetworkObject>());

        }

        player = null;
        

    }
}